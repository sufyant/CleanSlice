using CleanSlice.Application.Abstractions.Authorization;
using CleanSlice.Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;

namespace CleanSlice.Infrastructure.Authorization;

internal sealed class AuthorizationService(
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository,
    ILogger<AuthorizationService> logger) : IAuthorizationService
{
    public async Task<bool> HasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default)
    {
        try
        {
            var userPermissions = await GetUserPermissionsAsync(userId, cancellationToken);
            return userPermissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking permission {Permission} for user {UserId}", permission, userId);
            return false;
        }
    }

    public async Task<bool> HasRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default)
    {
        try
        {
            var userRoles = await GetUserRolesAsync(userId, cancellationToken);
            return userRoles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking role {Role} for user {UserId}", role, userId);
            return false;
        }
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var permissions = await permissionRepository.GetUserPermissionsAsync(userId, cancellationToken);
            return permissions.Select(p => p.Name.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting permissions for user {UserId}", userId);
            return [];
        }
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await roleRepository.GetUserRolesAsync(userId, cancellationToken);
            return roles.Select(r => r.Name.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting roles for user {UserId}", userId);
            return [];
        }
    }

    public async Task<bool> CanAccessResourceAsync(Guid userId, string resource, string action, CancellationToken cancellationToken = default)
    {
        try
        {
            var requiredPermission = $"{resource}.{action}".ToUpperInvariant();
            return await HasPermissionAsync(userId, requiredPermission, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking resource access {Resource}.{Action} for user {UserId}", resource, action, userId);
            return false;
        }
    }
}
