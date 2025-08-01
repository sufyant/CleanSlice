using CleanSlice.Domain.Users;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IUserTenantRepository : IBaseRepository<UserTenant>
{
    Task<UserTenant?> GetByUserAndTenantAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserTenant>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<UserTenant?> GetPrimaryTenantForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserTenant>> GetWithRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserMemberOfTenantAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default);
}
