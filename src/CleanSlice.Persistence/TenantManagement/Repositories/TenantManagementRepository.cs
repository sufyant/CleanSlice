using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Domain.Tenants;
using CleanSlice.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.TenantManagement.Repositories;

public sealed class TenantManagementRepository(TenantCatalogDbContext context) : ITenantManagementRepository
{
    public async Task<IReadOnlyList<Tenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default) => 
        await context.Tenants.AsNoTracking().ToListAsync(cancellationToken);
    
    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => 
        await context.Tenants.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await context.Tenants.AnyAsync(t => t.Name == name, cancellationToken);
    
    public async Task<Tenant> CreateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await context.Tenants.AddAsync(tenant, cancellationToken);
        return tenant;
    }
    
    public async Task<Tenant> DeactivateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        context.Tenants.Update(tenant);
        return await Task.FromResult(tenant);
    }
}
