using Asp.Versioning;
using CleanSlice.Api.Common;
using CleanSlice.Api.Controllers;
using CleanSlice.Shared.Contracts.Tenant.Invitations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers.Tenant;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/invitations")]
[Tags("Tenant - Invitations")]
[Produces("application/json")]
[Authorize] // Tenant Admin or Super Admin
public sealed class InvitationsController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(TenantInvitationListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [EndpointSummary("Get Tenant Invitations")]
    [EndpointDescription("Retrieves all invitations for the current tenant. Tenant admin or super admin access required.")]
    public async Task<IActionResult> GetInvitations([FromQuery] TenantInvitationListRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement get tenant invitations logic
        throw new NotImplementedException();
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateTenantInvitationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [EndpointSummary("Create Tenant Invitation")]
    [EndpointDescription("Creates an invitation to the current tenant. Tenant admin or super admin access required.")]
    public async Task<IActionResult> CreateInvitation([FromBody] CreateTenantInvitationRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement create tenant invitation logic
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TenantInvitationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get Invitation Details")]
    [EndpointDescription("Retrieves detailed information about a specific invitation in the current tenant.")]
    public async Task<IActionResult> GetInvitation([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement get invitation details logic
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Cancel Tenant Invitation")]
    [EndpointDescription("Cancels an invitation in the current tenant. Only invitations belonging to current tenant can be cancelled.")]
    public async Task<IActionResult> CancelInvitation([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement cancel tenant invitation logic
        throw new NotImplementedException();
    }

    [HttpPost("{id:guid}/resend")]
    [ProducesResponseType(typeof(ResendInvitationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Resend Invitation")]
    [EndpointDescription("Resends an invitation email with a new expiration date. Only pending invitations can be resent.")]
    public async Task<IActionResult> ResendInvitation([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement resend invitation logic
        throw new NotImplementedException();
    }
}
