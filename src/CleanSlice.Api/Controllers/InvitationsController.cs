using Asp.Versioning;
using CleanSlice.Api.Authorization;
using CleanSlice.Application.Features.Invitations.Commands.CancelInvitation;
using CleanSlice.Application.Features.Invitations.Commands.CreateInvitation;
using CleanSlice.Application.Features.Invitations.Queries.GetInvitation;
using CleanSlice.Application.Features.Invitations.Queries.GetInvitations;
using CleanSlice.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CleanSlice.Shared.Contracts.Invitations.Requests;
using CleanSlice.Shared.Contracts.Invitations.Responses;

namespace CleanSlice.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/invitations")]
[Tags("Invitations")]
[Produces("application/json")]
[Authorize]
public class InvitationsController(ISender sender, IMapper mapper) : BaseController
{
    [HttpPost]
    [HasPermission("INVITATIONS.CREATE")]
    [ProducesResponseType(typeof(InvitationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [EndpointSummary("Create User Invitation")]
    [EndpointDescription("Creates a new invitation for a user to join the tenant")]
    public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new CreateInvitationCommand(
            request.Email,
            request.RoleName,
            request.ExpirationDays);

        var result = await sender.Send(command, cancellationToken);

        // Map InvitationDto to InvitationResponse
        var response = mapper.Map<InvitationResponse>(result.Value);
        
        return result.IsSuccess ? Ok(response) : HandleFailure(result);
    }
    
    [HttpGet("{token}")]
    [HasPermission("INVITATIONS.READ")]
    [ProducesResponseType(typeof(InvitationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Get Invitation")]
    [EndpointDescription("Retrieves invitation details by token")]
    public async Task<IActionResult> GetInvitation(string token, CancellationToken cancellationToken)
    {
        var query = new GetInvitationQuery(token);
        var result = await sender.Send(query, cancellationToken);
        
        // Map InvitationDto to InvitationResponse
        var response = mapper.Map<InvitationResponse>(result.Value);

        return result.IsSuccess ? Ok(response) : HandleFailure(result);
    }
    
    [HttpDelete("{id:guid}")]
    [HasPermission("INVITATIONS.DELETE")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Cancel Invitation")]
    [EndpointDescription("Cancels/deletes an unused invitation")]
    public async Task<IActionResult> CancelInvitation(Guid id, CancellationToken cancellationToken)
    {
        var command = new CancelInvitationCommand(id);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }
    
    [HttpGet]
    [HasPermission("INVITATIONS.READ")]
    [ProducesResponseType(typeof(PagedResult<InvitationResponse>), StatusCodes.Status200OK)]
    [EndpointSummary("Get All Invitations")]
    [EndpointDescription("Retrieves all invitations for the current tenant")]
    public async Task<IActionResult> GetInvitations([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetInvitationsQuery(request.Page, request.PageSize, request.SearchTerm);
        var result = await sender.Send(query, cancellationToken);
        
        // Map PagedResult<InvitationDto> to PagedResult<InvitationResponse>
        var response = mapper.Map<PagedResult<InvitationResponse>>(result.Value);

        return result.IsSuccess ? Ok(response) : HandleFailure(result);
    }
}
