using Asp.Versioning;
using CleanSlice.Application.Features.Authentication.Commands.SyncUser;
using CleanSlice.Application.Features.Authentication.Queries.GetCurrentUser;
using CleanSlice.Shared.Contracts.Authentication.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/auth")]
[Tags("Authentication")]
[Produces("application/json")]
[Authorize]
public class AuthController(ISender sender) : BaseController
{
    [HttpPost("sync")]
    [Authorize]
    [ProducesResponseType(typeof(UserSyncResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Sync User from Azure Token")]
    [EndpointDescription("Syncs user information from Azure Entra ID token to local database")]
    public async Task<IActionResult> SyncUser(CancellationToken cancellationToken)
    {
        var command = new SyncUserCommand();
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
    
    [HttpGet("me")]
    [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Get Current User")]
    [EndpointDescription("Returns information about the currently authenticated user")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
}
