using CleanSlice.Domain.Tenants;

namespace CleanSlice.Application.Abstractions.Repositories.Management;

public interface ITenantManagementRepository
{
    Task<IReadOnlyList<Tenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default);
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Tenant> CreateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task<Tenant> DeactivateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default);
}
