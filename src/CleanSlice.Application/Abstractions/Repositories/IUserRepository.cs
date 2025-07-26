using CleanSlice.Domain.Users;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<User?> GetWithRolesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersWithRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);
}
