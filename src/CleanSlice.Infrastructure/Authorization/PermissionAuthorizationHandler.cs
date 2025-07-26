using CleanSlice.Application.Abstractions.Authorization;
using CleanSlice.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using IAuthorizationService = CleanSlice.Application.Abstractions.Authorization.IAuthorizationService;

namespace CleanSlice.Infrastructure.Authorization;

public sealed class PermissionAuthorizationHandler(
    IAuthorizationService authorizationService,
    ILogger<PermissionAuthorizationHandler> logger)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Önce JWT token'dan permission'ları kontrol et (hızlı yol)
        var permissions = context.User.GetPermissions();
        if (permissions.Contains(requirement.Permission, StringComparer.OrdinalIgnoreCase))
        {
            logger.LogDebug("Permission {Permission} found in JWT token for user", requirement.Permission);
            context.Succeed(requirement);
            return;
        }

        // JWT'de yoksa veritabanından kontrol et (güvenli yol)
        try
        {
            var userId = context.User.GetUserId();
            var hasPermission = await authorizationService.HasPermissionAsync(userId, requirement.Permission);

            if (hasPermission)
            {
                logger.LogDebug("Permission {Permission} granted from database for user {UserId}", requirement.Permission, userId);
                context.Succeed(requirement);
            }
            else
            {
                logger.LogWarning("Permission {Permission} denied for user {UserId}", requirement.Permission, userId);
                // context.Fail() otomatik olarak çağrılır, manuel çağırmaya gerek yok
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking permission {Permission}", requirement.Permission);
            // Exception durumunda da permission reddedilir
        }
    }
}
