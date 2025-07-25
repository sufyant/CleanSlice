using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Tenants.Commands.UpdateTenant;

internal sealed class UpdateTenantCommandHandler(
    ITenantManagementRepository tenantManagementRepository
    ): ICommandHandler<UpdateTenantCommand>
{
    public async Task<Result> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await tenantManagementRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return TenantErrors.NotFound;
        }

        tenant.Update(
            request.TenantId,
            request.Name,
            request.Domain,
            request.Slug,
            request.ConnectionString
        );

        if (await tenantManagementRepository.ExistsByNameAsync(tenant.Name, cancellationToken))
        {
            return TenantErrors.AlreadyExists;
        }
        
        if (await tenantManagementRepository.ExistsBySlugAsync(tenant.Slug, cancellationToken))
        {
            return TenantErrors.AlreadyExists;
        }

        await tenantManagementRepository.UpdateTenantAsync(tenant, cancellationToken);

        return Result.Success();
    }
}
