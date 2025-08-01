using CleanSlice.Domain.Users;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IPermissionRepository : IBaseRepository<Permission>
{
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    // Pagination methods
    Task<PagedResult<Permission>> GetPagedPermissionsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<Permission>> GetPagedPermissionsByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default);
}
