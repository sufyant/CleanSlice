using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class RolePermissionRepository(ApplicationDbContext dbContext) : BaseRepository<RolePermission>(dbContext), IRolePermissionRepository
{
    public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await dbContext.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.PermissionId == permissionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<RolePermission?> GetByRoleAndPermissionAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await dbContext.RolePermissions
            .AsNoTracking()
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await dbContext.RolePermissions
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);
    }

    public async Task<int> GetPermissionCountForRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.RolePermissions
            .CountAsync(rp => rp.RoleId == roleId, cancellationToken);
    }
}
