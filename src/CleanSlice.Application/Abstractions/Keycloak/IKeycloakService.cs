namespace CleanSlice.Application.Abstractions.Keycloak;

public interface IKeycloakService
{
    Task<string?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> CreateUserAsync(string email, string firstName, string lastName, string password, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserAsync(string userId, string email, string firstName, string lastName, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> AssignRoleToUserAsync(string userId, string roleName, CancellationToken cancellationToken = default);
    Task<bool> RemoveRoleFromUserAsync(string userId, string roleName, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> CreateRoleAsync(string roleName, string description, CancellationToken cancellationToken = default);
    Task<bool> DeleteRoleAsync(string roleName, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<bool> SetUserPasswordAsync(string userId, string password, bool temporary = false, CancellationToken cancellationToken = default);
    Task<bool> EnableUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DisableUserAsync(string userId, CancellationToken cancellationToken = default);
}
