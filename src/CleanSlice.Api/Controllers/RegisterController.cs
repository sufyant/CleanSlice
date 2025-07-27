using Asp.Versioning;
using CleanSlice.Application.Features.Registration.Commands.RegisterFromInvite;
using CleanSlice.Application.Features.Registration.Queries.ResolveInvitation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CleanSlice.Shared.Contracts.Registration.Requests;
using CleanSlice.Shared.Contracts.Registration.Responses;

namespace CleanSlice.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/register")]
[Tags("Registration")]
[Produces("application/json")]
[Authorize]
public class RegisterController(ISender sender, IMapper mapper) : BaseController
{
    [HttpGet("invitations/resolve")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(InvitationDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Resolve Invitation Token")]
    [EndpointDescription("Validates invitation token and returns invitation details for registration form")]
    public async Task<IActionResult> ResolveInvitation([FromQuery] string token, CancellationToken cancellationToken)
    {
        var query = new ResolveInvitationQuery(token);
        var result = await sender.Send(query, cancellationToken);

        // Map InvitationDetailsDto to InvitationDetailsResponse
        var response = mapper.Map<InvitationDetailsResponse>(result.Value);
        
        return result.IsSuccess ? Ok(response) : HandleFailure(result);
    }
    
    [HttpPost("from-invite")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Register from Invitation")]
    [EndpointDescription("Registers a new user using invitation token and creates user in both local DB and Azure Entra ID")]
    public async Task<IActionResult> RegisterFromInvitation([FromBody] RegisterFromInviteRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new RegisterFromInviteCommand(
            request.Token,
            request.Email,
            request.FirstName,
            request.LastName);

        var result = await sender.Send(command, cancellationToken);
        
        // Map RegistrationDto to RegistrationResponse
        var response = mapper.Map<RegistrationResponse>(result.Value);

        return result.IsSuccess ? Ok(response) : HandleFailure(result);
    }
}
