using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Common.Enums;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByExternalIdentityIdAsync(string externalIdentityId, LoginProvider provider = LoginProvider.Local, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.ExternalIdentityId.Value == externalIdentityId &&
                                    u.ExternalIdentityId.Provider == provider, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId))
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetWithTenantsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.UserTenants)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetWithTenantsAndRolesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.UserTenants)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersWithRoleInTenantAsync(Guid roleId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId) &&
                       u.UserRoles.Any(ur => ur.RoleId == roleId && ur.TenantId == tenantId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Email.Value == email, cancellationToken);
    }

    public async Task<bool> ExistsByExternalIdentityIdAsync(string externalIdentityId, LoginProvider provider = LoginProvider.Local, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.ExternalIdentityId.Value == externalIdentityId &&
                          u.ExternalIdentityId.Provider == provider, cancellationToken);
    }
}
