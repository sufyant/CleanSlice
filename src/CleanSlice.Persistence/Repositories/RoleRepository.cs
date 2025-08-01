using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Results;
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

    // Pagination methods
    public async Task<PagedResult<Role>> GetPagedRolesAsync(Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Roles
            .AsNoTracking()
            .Where(r => r.TenantId == tenantId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(r =>
                r.Name.Value.Contains(request.SearchTerm) ||
                r.Description.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderBy(r => r.Name.Value);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<PagedResult<Role>> GetPagedRolesWithPermissionsAsync(Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Roles
            .AsNoTracking()
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Where(r => r.TenantId == tenantId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(r =>
                r.Name.Value.Contains(request.SearchTerm) ||
                r.Description.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderBy(r => r.Name.Value);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
