using CleanSlice.Domain.Users;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IPermissionRepository : IBaseRepository<Permission>
{
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
}
