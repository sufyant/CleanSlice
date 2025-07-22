using System.Data;
using CleanSlice.Application.Abstractions.Data;
using CleanSlice.Domain.Outbox;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace CleanSlice.Persistence.UnitOfWork;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return _transaction.GetDbTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Add domain events as outbox messages
        AddDomainEventsAsOutboxMessages(cancellationToken);

        // Save changes to the database
        return await dbContext.SaveChangesAsync(cancellationToken);
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
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }),
                DateTimeOffset.UtcNow
                ))
            .ToList();

        dbContext.AddRange(outboxMessages, cancellationToken);
    }
}
