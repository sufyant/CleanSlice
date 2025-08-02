using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Entities;
using CleanSlice.Shared.Exceptions.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CleanSlice.Persistence.Repositories;

internal abstract class BaseRepository<T>(ApplicationDbContext dbContext) : IBaseRepository<T>
    where T : BaseEntity
{
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public virtual async Task<T> GetByIdRequiredAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        return entity ?? throw new EntityNotFoundException($"{typeof(T).Name} with ID {id} was not found");
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public virtual IQueryable<T> Query() =>
        dbContext.Set<T>().AsQueryable();

    public virtual IQueryable<T> QueryAsNoTracking() =>
        dbContext.Set<T>().AsQueryable().AsNoTracking();

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) =>
        await dbContext.Set<T>().AsNoTracking().AnyAsync(e => e.Id == id, cancellationToken);

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken) =>
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);

    public virtual void Update(T entity) =>
        dbContext.Set<T>().Update(entity);

    public virtual void Delete(T entity) =>
        dbContext.Set<T>().Remove(entity);

    public virtual async Task<List<TResult>> SqlQuery<TResult>(string sql, params object[] parameters) where TResult : class =>
        await dbContext.Database.SqlQuery<TResult>(FormattableStringFactory.Create(sql, parameters)).ToListAsync();
}
