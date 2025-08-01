using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class UserTenantRepository(ApplicationDbContext dbContext) : BaseRepository<UserTenant>(dbContext), IUserTenantRepository
{
    public async Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserTenants
            .AsNoTracking()
            .Where(ut => ut.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserTenant>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserTenants
            .AsNoTracking()
            .Where(ut => ut.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserTenant?> GetByUserAndTenantAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserTenants
            .AsNoTracking()
            .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId, cancellationToken);
    }

    public async Task<UserTenant?> GetPrimaryTenantAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserTenants
            .AsNoTracking()
            .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.IsPrimary, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserTenants
            .AnyAsync(ut => ut.UserId == userId && ut.TenantId == tenantId, cancellationToken);
    }

    public async Task<int> GetTenantCountForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserTenants
            .CountAsync(ut => ut.UserId == userId, cancellationToken);
    }
}
