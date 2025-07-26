using Asp.Versioning;
using AutoMapper;
using CleanSlice.Api.Authorization;
using CleanSlice.Application.Features.Roles.Commands.CreateRole;
using CleanSlice.Shared.Contracts.Roles.Requests;
using CleanSlice.Shared.Contracts.Roles.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/roles")]
[Tags("Role Management")]
[Produces("application/json")]
[Authorize]
public class RolesController(ISender sender, IMapper mapper) : BaseController
{
    [HttpPost]
    [HasPermission("ROLES.WRITE")]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Create Role")]
    [EndpointDescription("Create a new role with specified permissions")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateRoleCommand(
            request.Name,
            request.Description,
            request.Permissions);

        var result = await sender.Send(command, cancellationToken);
        
        var response = mapper.Map<RoleResponse>(result.Value);

        return result.IsSuccess 
            ? Ok(response)
            : BadRequest(result.Error);
    }
}
