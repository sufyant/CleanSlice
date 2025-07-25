using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Application.Features.Tenants.DTOs;
using CleanSlice.Domain.Tenants;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Features.Tenants.Queries.GetTenant;

public sealed class GetTenantQueryHandler(
    ITenantManagementRepository tenantManagementRepository
    )
    : IQueryHandler<GetTenantQuery, TenantDto>
{
    public async Task<Result<TenantDto>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await tenantManagementRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return TenantErrors.TenantNotFound;
        }

        var response = new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.Domain,
            tenant.Slug,
            tenant.ConnectionString
        );

        return Result.Success(response);
    }
}
