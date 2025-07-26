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

    public static IEnumerable<string> GetRoles(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return [];

        // Try to get roles from realm_access claim (Keycloak format)
        var realmAccessClaim = principal.FindFirstValue("realm_access");
        if (!string.IsNullOrEmpty(realmAccessClaim))
        {
            try
            {
                var realmAccess = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(realmAccessClaim);
                if (realmAccess != null && realmAccess.TryGetValue("roles", out var rolesObj))
                {
                    var rolesElement = (System.Text.Json.JsonElement)rolesObj;
                    if (rolesElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        return rolesElement.EnumerateArray().Select(r => r.GetString()).Where(r => !string.IsNullOrEmpty(r))!;
                    }
                }
            }
            catch
            {
                // Fall back to standard role claims
            }
        }

        // Fall back to standard role claims
        return principal.FindAll(ClaimTypes.Role).Select(c => c.Value);
    }

    public static IEnumerable<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return [];

        // Get permissions from custom claims
        return principal.FindAll("permission").Select(c => c.Value);
    }
}
