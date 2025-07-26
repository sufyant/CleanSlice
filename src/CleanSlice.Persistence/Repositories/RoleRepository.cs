using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class RoleRepository(ApplicationDbContext dbContext) : BaseRepository<Role>(dbContext), IRoleRepository
{
    public async Task<Role?> GetByNameAsync(string name, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name.Value.Equals(name, StringComparison.InvariantCultureIgnoreCase) && r.TenantId == tenantId, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AsNoTracking()
            .Where(r => r.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Role?> GetWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AsNoTracking()
            .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AnyAsync(r => r.Name.Value.Equals(name, StringComparison.InvariantCultureIgnoreCase) && r.TenantId == tenantId, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AsNoTracking()
            .Where(r => r.IsSystemRole)
            .ToListAsync(cancellationToken);
    }
}
