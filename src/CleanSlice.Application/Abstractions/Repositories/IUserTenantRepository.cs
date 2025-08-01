using CleanSlice.Domain.Users;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IUserTenantRepository : IBaseRepository<UserTenant>
{
    Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserTenant>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<UserTenant?> GetByUserAndTenantAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<UserTenant?> GetPrimaryTenantAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<int> GetTenantCountForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
