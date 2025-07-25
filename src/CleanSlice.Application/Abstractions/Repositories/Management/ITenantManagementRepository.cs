using CleanSlice.Domain.Tenants;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Abstractions.Repositories.Management;

public interface ITenantManagementRepository
{
    Task<PagedResult<Tenant>> GetPagedTenantsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Tenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default);
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Tenant> CreateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task<Tenant> UpdateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task<Tenant> DeactivateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default);
}
