using Asp.Versioning;
using AutoMapper;
using CleanSlice.Application.Features.Tenants.Commands.CreateTenant;
using CleanSlice.Application.Features.Tenants.Commands.UpdateTenant;
using CleanSlice.Application.Features.Tenants.Queries.GetTenant;
using CleanSlice.Shared.Contracts.Tenants.Requests;
using CleanSlice.Shared.Contracts.Tenants.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
[Route("v{version:apiVersion}/tenants")]
public class TenantsController(ISender sender, IMapper mapper) : BaseController
{
    [HttpGet]
    [Route("{tenantId:guid}")]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status200OK)]
    [EndpointSummary("Get Tenant by ID")]
    [EndpointDescription("Get a tenant by its ID")]
    public async Task<IActionResult> GetTenant([FromRoute] Guid tenantId, CancellationToken cancellationToken)
    {
        var query = new GetTenantQuery(tenantId);

        var result = await sender.Send(query, cancellationToken);

        var response = mapper.Map<TenantResponse>(result);

        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [EndpointSummary("Create Tenant")]
    [EndpointDescription("Create a new tenant")]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateTenantCommand>(request);
        
        var result = await sender.Send(command, cancellationToken);

        return Ok(result.Value);
    }
    
    [HttpPut]
    [EndpointSummary("Update Tenant")]
    [EndpointDescription("Update an existing tenant")]
    public async Task<IActionResult> UpdateTenant([FromBody] UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateTenantCommand>(request);
        
        await sender.Send(command, cancellationToken);

        return Ok();
    }
    
}
