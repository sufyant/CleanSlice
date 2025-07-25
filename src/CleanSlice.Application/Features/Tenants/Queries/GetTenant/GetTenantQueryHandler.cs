using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Application.Features.Tenants.DTOs;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Tenants.Queries.GetTenant;

internal sealed class GetTenantQueryHandler(
    ITenantManagementRepository tenantManagementRepository
    ) : IQueryHandler<GetTenantQuery, TenantDto>
{
    public async Task<Result<TenantDto>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await tenantManagementRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return TenantErrors.NotFound;
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
