namespace CleanSlice.Application.Abstractions.Repositories;

public interface IBaseRepository<T> where T : class
{
    // Query methods
    IQueryable<T> Query();
    IQueryable<T> QueryAsNoTracking();
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T> GetByIdRequiredAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    // Write operations
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);

    // SQL operations
    Task<List<TResult>> SqlQuery<TResult>(string sql, params object[] parameters) where TResult : class;
}
