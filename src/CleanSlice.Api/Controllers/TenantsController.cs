using Asp.Versioning;
using AutoMapper;
using CleanSlice.Application.Features.Tenants.Commands.CreateTenant;
using CleanSlice.Application.Features.Tenants.Commands.UpdateTenant;
using CleanSlice.Application.Features.Tenants.Queries.GetTenant;
using CleanSlice.Application.Features.Tenants.Queries.GetTenants;
using CleanSlice.Shared.Contracts.Tenants.Requests;
using CleanSlice.Shared.Contracts.Tenants.Responses;
using CleanSlice.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("v{version:apiVersion}/tenants")]
[Tags("Tenant Management")]
[Produces("application/json")]
public class TenantsController(ISender sender, IMapper mapper) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TenantResponse>), StatusCodes.Status200OK)]
    [EndpointSummary("Get Tenants")]
    [EndpointDescription("Get tenants with pagination and optional search")]
    public async Task<IActionResult> GetTenants([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        var query = new GetTenantsQuery(request.Page, request.PageSize, request.SearchTerm);
        var result = await sender.Send(query, cancellationToken);

        // Map TenantDto to TenantResponse
        var response = mapper.Map<PagedResult<TenantResponse>>(result.Value);

        return Ok(response);
    }

    [HttpGet]
    [Route("{tenantId:guid}")]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status200OK)]
    [EndpointSummary("Get Tenant by ID")]
    [EndpointDescription("Get a tenant by its ID")]
    public async Task<IActionResult> GetTenant([FromRoute] Guid tenantId, CancellationToken cancellationToken)
    {
        var query = new GetTenantQuery(tenantId);
        var result = await sender.Send(query, cancellationToken);

        // Map TenantDto to TenantResponse
        var response = mapper.Map<TenantResponse>(result);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [EndpointSummary("Create Tenant")]
    [EndpointDescription("Create a new tenant")]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
    {
        // Map CreateTenantRequest to CreateTenantCommand
        var command = mapper.Map<CreateTenantCommand>(request);

        var result = await sender.Send(command, cancellationToken);

        return Ok(result.Value);
    }

    [HttpPut]
    [EndpointSummary("Update Tenant")]
    [EndpointDescription("Update an existing tenant")]
    public async Task<IActionResult> UpdateTenant([FromBody] UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        // Map UpdateTenantRequest to UpdateTenantCommand
        var command = mapper.Map<UpdateTenantCommand>(request);

        await sender.Send(command, cancellationToken);

        return Ok();
    }
}
