using Asp.Versioning;
using CleanSlice.Api.Common;
using CleanSlice.Api.Controllers;
using CleanSlice.Shared.Contracts.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers.Admin;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/admin/users")]
[Tags("Admin - Users")]
[Produces("application/json")]
[Authorize] // Super Admin only
public sealed class AdminUsersController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(AdminUserListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [EndpointSummary("Get All Users (Cross-Tenant)")]
    [EndpointDescription("Retrieves all users across all tenants. Super admin access required.")]
    public async Task<IActionResult> GetUsers([FromQuery] AdminUserListRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement get all users logic
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AdminUserDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get User Details")]
    [EndpointDescription("Retrieves detailed user information including all tenant memberships and roles.")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement get user details logic
        throw new NotImplementedException();
    }

    [HttpPut("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Activate User")]
    [EndpointDescription("Activates a deactivated user account. User will be able to login again.")]
    public async Task<IActionResult> ActivateUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement activate user logic
        throw new NotImplementedException();
    }

    [HttpPut("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Deactivate User")]
    [EndpointDescription("Deactivates a user account. User will not be able to login until reactivated.")]
    public async Task<IActionResult> DeactivateUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement deactivate user logic
        throw new NotImplementedException();
    }

    [HttpGet("super-admins")]
    [ProducesResponseType(typeof(SuperAdminListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [EndpointSummary("Get Super Admins")]
    [EndpointDescription("Retrieves list of all super administrators in the system.")]
    public async Task<IActionResult> GetSuperAdmins(CancellationToken cancellationToken)
    {
        // TODO: Implement get super admins logic
        throw new NotImplementedException();
    }
}
