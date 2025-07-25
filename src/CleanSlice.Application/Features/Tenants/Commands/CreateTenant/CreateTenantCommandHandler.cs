using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Domain.Tenants;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Tenants.Commands.CreateTenant;

internal sealed class CreateTenantCommandHandler(
    ITenantManagementRepository tenantManagementRepository 
    ) : ICommandHandler<CreateTenantCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantId = Guid.NewGuid();
        var tenant = Tenant.Create(
            tenantId,
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

        tenant = await tenantManagementRepository.CreateTenantAsync(tenant, cancellationToken);

        return Result.Success(tenant.Id);
    }
}
