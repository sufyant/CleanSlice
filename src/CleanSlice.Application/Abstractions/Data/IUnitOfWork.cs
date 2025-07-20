using System.Data;

namespace CleanSlice.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<IDbTransaction>  BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
