using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Application.Features.Tenants.DTOs;
using CleanSlice.Shared.Results;
using AutoMapper;

namespace CleanSlice.Application.Features.Tenants.Queries.GetTenants;

internal sealed class GetTenantsQueryHandler(
    ITenantManagementRepository tenantManagementRepository,
    IMapper mapper
    ) : IQueryHandler<GetTenantsQuery, PagedResult<TenantDto>>
{
    public async Task<Result<PagedResult<TenantDto>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var tenants = await tenantManagementRepository.GetPagedTenantsAsync(
            request.Request,
            cancellationToken
        );

        // Map Tenant entities to TenantDto
        var tenantDtos = mapper.Map<List<TenantDto>>(tenants.Items);

        // Create new PagedResult with mapped DTOs
        var result = PagedResult<TenantDto>.Create(
            tenantDtos,
            tenants.TotalCount,
            tenants.Page,
            tenants.PageSize
        );

        return Result.Success(result);
    }
}
