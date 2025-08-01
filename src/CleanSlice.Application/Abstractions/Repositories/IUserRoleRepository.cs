using CleanSlice.Domain.Users;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IUserRoleRepository : IBaseRepository<UserRole>
{
    Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetByUserAndTenantAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<UserRole?> GetByUserAndRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    Task<int> GetRoleCountForUserInTenantAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default);
}
