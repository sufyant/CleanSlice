using CleanSlice.Domain.Users;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<Role?> GetWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default);
}
