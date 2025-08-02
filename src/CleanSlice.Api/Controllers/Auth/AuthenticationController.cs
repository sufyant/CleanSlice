using Asp.Versioning;
using CleanSlice.Api.Common;
using CleanSlice.Shared.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers.Auth;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/auth")]
[Tags("Authentication")]
[Produces("application/json")]
public sealed class AuthenticationController : BaseController
{
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("User Login with Email and Password")]
    [EndpointDescription("Authenticates user with email and password credentials. Returns JWT token and user information.")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement login logic
        throw new NotImplementedException();
    }

    [HttpPost("external/google")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ExternalLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Google OAuth Login")]
    [EndpointDescription("Authenticates user with Google OAuth token. Creates user if not exists and has valid invitation.")]
    public async Task<IActionResult> GoogleLogin([FromBody] ExternalLoginRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement Google OAuth logic
        throw new NotImplementedException();
    }

    [HttpPost("external/microsoft")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ExternalLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Microsoft OAuth Login")]
    [EndpointDescription("Authenticates user with Microsoft OAuth token. Creates user if not exists and has valid invitation.")]
    public async Task<IActionResult> MicrosoftLogin([FromBody] ExternalLoginRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement Microsoft OAuth logic
        throw new NotImplementedException();
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("User Logout")]
    [EndpointDescription("Logs out the current user and invalidates the JWT token.")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        // TODO: Implement logout logic
        throw new NotImplementedException();
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Refresh JWT Token")]
    [EndpointDescription("Refreshes expired JWT token using refresh token. Returns new access token.")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement token refresh logic
        throw new NotImplementedException();
    }
}
