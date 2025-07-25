using System.ComponentModel;
using Asp.Versioning;
using AutoMapper;
using CleanSlice.Application.Features.Tenants.Queries.GetTenant;
using CleanSlice.Shared.Contracts.Tenants.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/tenants")]
public class TenantsController(ISender sender, IMapper mapper) : BaseController
{
    [Description("Get a tenant by its unique identifier")]
    [HttpGet]
    [Route("{tenantId:guid}")]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTenant([FromRoute] Guid tenantId, CancellationToken cancellationToken)
    {
        var query = new GetTenantQuery(tenantId);

        var result = await sender.Send(query, cancellationToken);
        
        var response = mapper.Map<TenantResponse>(result);

        return Ok(response);
    }
    
    
}
