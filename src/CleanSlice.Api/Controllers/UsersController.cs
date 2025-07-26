using Asp.Versioning;
using AutoMapper;
using CleanSlice.Api.Authorization;
using CleanSlice.Application.Features.Users.Commands.CreateUser;
using CleanSlice.Application.Features.Users.Queries.GetUsers;
using CleanSlice.Shared.Contracts.Users.Requests;
using CleanSlice.Shared.Contracts.Users.Responses;
using CleanSlice.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/users")]
[Tags("User Management")]
[Produces("application/json")]
[Authorize]
public class UsersController(ISender sender, IMapper mapper) : BaseController
{
    [HttpGet]
    [HasPermission("USERS.READ")]
    [ProducesResponseType(typeof(PagedResult<UserResponse>), StatusCodes.Status200OK)]
    [EndpointSummary("Get Users")]
    [EndpointDescription("Get users with pagination and optional search")]
    public async Task<IActionResult> GetUsers([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(request.Page, request.PageSize, request.SearchTerm);
        var result = await sender.Send(query, cancellationToken);
        
        var response = mapper.Map<UserResponse>(result.Value);

        return result.IsSuccess 
            ? Ok(response) 
            : BadRequest(result.Error);
    }

    [HttpPost]
    [HasPermission("USERS.WRITE")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Create User")]
    [EndpointDescription("Create a new user with specified roles")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password,
            request.Roles);

        var result = await sender.Send(command, cancellationToken);
        
        var response = mapper.Map<UserResponse>(result.Value);

        return result.IsSuccess 
            ? Ok(response)
            : BadRequest(result.Error);
    }
}
