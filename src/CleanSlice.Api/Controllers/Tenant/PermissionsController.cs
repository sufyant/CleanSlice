using CleanSlice.Api.Common;
using CleanSlice.Api.Controllers;
using CleanSlice.Shared.Contracts.Tenant.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers.Tenant;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/permissions")]
[Tags("Tenant - Permissions")]
[Produces("application/json")]
[Authorize] // Tenant member access
public sealed class PermissionsController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(PermissionListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [EndpointSummary("Get Available Permissions")]
    [EndpointDescription("Retrieves all available permissions that can be assigned to roles in the system.")]
    public async Task<IActionResult> GetPermissions([FromQuery] PermissionListRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement get permissions logic
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PermissionDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get Permission Details")]
    [EndpointDescription("Retrieves detailed information about a specific permission including which roles have it.")]
    public async Task<IActionResult> GetPermission([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement get permission details logic
        throw new NotImplementedException();
    }

    [HttpGet("by-category")]
    [ProducesResponseType(typeof(PermissionsByCategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [EndpointSummary("Get Permissions by Category")]
    [EndpointDescription("Retrieves permissions grouped by category for easier role management.")]
    public async Task<IActionResult> GetPermissionsByCategory(CancellationToken cancellationToken)
    {
        // TODO: Implement get permissions by category logic
        throw new NotImplementedException();
    }
}
