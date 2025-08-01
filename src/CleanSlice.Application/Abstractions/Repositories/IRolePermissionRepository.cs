using CleanSlice.Domain.Users;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IRolePermissionRepository : IBaseRepository<RolePermission>
{
    Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default);
    Task<RolePermission?> GetByRoleAndPermissionAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
    Task<int> GetPermissionCountForRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
}
