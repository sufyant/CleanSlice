using System.Data;
using CleanSlice.Application.Abstractions.Data;
using CleanSlice.Domain.Outbox;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Entities;
using CleanSlice.Shared.Exceptions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace CleanSlice.Persistence.UnitOfWork;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            return _transaction.GetDbTransaction(); // Already active transaction

        _transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return _transaction.GetDbTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new TransactionException("No active transaction to commit");

        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new TransactionException("No active transaction to rollback");

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Add domain events as outbox messages
            AddDomainEventsAsOutboxMessages(cancellationToken);

            // Save changes to the database
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Let the behavior handle the exception
            // The behavior will convert it to Result.Failure
            throw new TransactionException("Failed to save changes to database", ex);
        }
    }

    private void AddDomainEventsAsOutboxMessages(CancellationToken cancellationToken = default)
    {
        var outboxMessages = dbContext.ChangeTracker
            .Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => OutboxMessage.Create(
                Guid.NewGuid(),
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
            ))
            .ToList();

        if (outboxMessages.Any())
        {
            dbContext.OutboxMessages.AddRange(outboxMessages);
        }
    }
}


