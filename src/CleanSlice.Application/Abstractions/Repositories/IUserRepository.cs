using CleanSlice.Domain.Common.Enums;
using CleanSlice.Domain.Users;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByExternalIdentityIdAsync(string externalIdentityId, LoginProvider provider = LoginProvider.Local, CancellationToken cancellationToken = default);
    Task<User> GetByExternalIdentityIdRequiredAsync(string externalIdentityId, LoginProvider provider = LoginProvider.Local, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> GetByEmailRequiredAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<User?> GetWithTenantsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetWithTenantsAndRolesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersWithRoleInTenantAsync(Guid roleId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByExternalIdentityIdAsync(string externalIdentityId, LoginProvider provider = LoginProvider.Local, CancellationToken cancellationToken = default);

    // Super Admin methods
    Task<IEnumerable<User>> GetSuperAdminsAsync(CancellationToken cancellationToken = default);
    Task<int> GetSuperAdminCountAsync(CancellationToken cancellationToken = default);
    Task<bool> HasAnySuperAdminAsync(CancellationToken cancellationToken = default);

    // Authentication methods
    Task<User?> GetByEmailForAuthenticationAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetLocalUsersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetExternalUsersAsync(LoginProvider provider, CancellationToken cancellationToken = default);

    // Pagination methods
    Task<PagedResult<User>> GetPagedUsersAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetPagedUsersByTenantAsync(Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetPagedUsersWithRoleAsync(Guid roleId, Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default);
}
