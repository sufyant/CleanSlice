using Asp.Versioning;
using CleanSlice.Api.Common;
using CleanSlice.Api.Controllers;
using CleanSlice.Shared.Contracts.Public.Invitations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers.Public;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/invitations")]
[Tags("Public - Invitations")]
[Produces("application/json")]
[AllowAnonymous]
public sealed class PublicInvitationsController : BaseController
{
    [HttpGet("{token}")]
    [ProducesResponseType(typeof(InvitationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get Invitation Details")]
    [EndpointDescription("Retrieves invitation details by token. Used to display invitation information before acceptance.")]
    public async Task<IActionResult> GetInvitationDetails([FromRoute] string token, CancellationToken cancellationToken)
    {
        // TODO: Implement get invitation details logic
        throw new NotImplementedException();
    }

    [HttpPost("{token}/accept")]
    [ProducesResponseType(typeof(AcceptInvitationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [EndpointSummary("Accept Invitation")]
    [EndpointDescription("Accepts an invitation and creates user account. For local users, password is required. For external users, OAuth token is required.")]
    public async Task<IActionResult> AcceptInvitation([FromRoute] string token, [FromBody] AcceptInvitationRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement accept invitation logic
        throw new NotImplementedException();
    }

    [HttpPost("{token}/decline")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Decline Invitation")]
    [EndpointDescription("Declines an invitation. The invitation will be marked as declined and cannot be used.")]
    public async Task<IActionResult> DeclineInvitation([FromRoute] string token, CancellationToken cancellationToken)
    {
        // TODO: Implement decline invitation logic
        throw new NotImplementedException();
    }
}
