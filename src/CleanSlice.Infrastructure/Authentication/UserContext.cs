using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Shared.Exceptions.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace CleanSlice.Infrastructure.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new AuthenticationException("User context is unavailable");

    public Guid TenantId => 
        httpContextAccessor
            .HttpContext?
            .User
            .GetTenantId() ??
        throw new AuthenticationException("User context is unavailable");
    
    public string IdentityId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetIdentityId() ??
        throw new AuthenticationException("User context is unavailable");
    
    public string Email =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetEmail() ??
        throw new AuthenticationException("User context is unavailable");
            
    public string Name =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetName() ??
        throw new AuthenticationException("User context is unavailable");

    public IEnumerable<string> Roles =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetRoles() ?? [];

    public IEnumerable<string> Permissions =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetPermissions() ?? [];

    public bool HasRole(string role)
    {
        return Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }

    public bool HasPermission(string permission)
    {
        return Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
    }
}
