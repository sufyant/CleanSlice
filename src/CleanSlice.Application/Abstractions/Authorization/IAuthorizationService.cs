namespace CleanSlice.Application.Abstractions.Authorization;

public interface IAuthorizationService
{
    Task<bool> HasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default);
    Task<bool> HasRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> CanAccessResourceAsync(Guid userId, string resource, string action, CancellationToken cancellationToken = default);
}
