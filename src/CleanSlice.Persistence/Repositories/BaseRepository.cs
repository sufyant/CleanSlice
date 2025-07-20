using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CleanSlice.Persistence.Repositories;

internal abstract class BaseRepository<T>(ApplicationDbContext dbContext) : IBaseRepository<T>
    where T : BaseEntity
{
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Set<T>().ToListAsync(cancellationToken);

    public virtual IQueryable<T> Query() =>
        dbContext.Set<T>().AsQueryable();

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) =>
        await dbContext.Set<T>().AnyAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken) =>
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);

    public virtual void Update(T entity) =>
        dbContext.Set<T>().Update(entity);

    public virtual void Delete(T entity) =>
        dbContext.Set<T>().Remove(entity);

    public virtual async Task<List<TResult>> SqlQuery<TResult>(string sql, params object[] parameters) where TResult : class =>
        await dbContext.Database.SqlQuery<TResult>(FormattableStringFactory.Create(sql, parameters)).ToListAsync();
}
