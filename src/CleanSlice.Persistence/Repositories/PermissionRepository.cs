using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class PermissionRepository(ApplicationDbContext dbContext) : BaseRepository<Permission>(dbContext), IPermissionRepository
{
    public async Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name.Value.Equals(name, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .Where(p => p.Category == category)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .Where(p => p.RolePermissions.Any(rp => rp.Role.UserRoles.Any(ur => ur.UserId == userId)))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AnyAsync(p => p.Name.Value.Equals(name, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
    }

    public async Task<IEnumerable<string>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .Select(p => p.Category)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}
