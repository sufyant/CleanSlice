using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IRepository<T> where T : class
{
    // Query methods
    IQueryable<T> Query();
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    // Write operations
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);

    // SQL operations
    Task<Result<IQueryable<TResult>>> SqlQuery<TResult>(string sql, params object[] parameters) where TResult : class;
}
