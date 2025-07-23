using System.Security.Claims;
using CleanSlice.Shared.Exceptions.Infrastructure;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CleanSlice.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new AuthenticationException("User id is unavailable");
    }
    
    public static Guid GetTenantId(this ClaimsPrincipal? principal)
    {
        string? tenantId = principal?.FindFirstValue("tenant");
        return Guid.TryParse(tenantId, out Guid parsedTenantId) ? parsedTenantId : 
            throw new AuthenticationException("Tenant id is unavailable");
    }

    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new AuthenticationException("User identity is unavailable");
    }
    
    public static string GetEmail(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.Email) ?? 
               throw new AuthenticationException("Email is unavailable");
    }

    public static string GetName(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.Name) ?? 
               throw new AuthenticationException("Name is unavailable");
    }
}
