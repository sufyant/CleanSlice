using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Domain.Tenants;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.TenantManagement.Repositories;

public sealed class TenantManagementRepository(TenantCatalogDbContext context) : ITenantManagementRepository
{
    public async Task<PagedResult<Tenant>> GetPagedTenantsAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        // Build the query
        var query = context.Tenants.AsNoTracking();

        // Apply search filter if searchTerm is provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(t =>
                t.Name.Value.Contains(request.SearchTerm) ||
                t.Slug.Value.Contains(request.SearchTerm) ||
                t.Domain.Value.Contains(request.SearchTerm)
            );
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderBy(t => t.Name.Value);
        }

        // Apply pagination and return result
        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<IReadOnlyList<Tenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default) =>
        await context.Tenants.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Tenants.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await context.Tenants.AnyAsync(t => t.Name.Value == name, cancellationToken);

    public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default) =>
        await context.Tenants.AnyAsync(t => t.Slug.Value == slug, cancellationToken);

    public async Task<Tenant> CreateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await context.Tenants.AddAsync(tenant, cancellationToken);
        return tenant;
    }

    public async Task<Tenant> UpdateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        context.Tenants.Update(tenant);
        return await Task.FromResult(tenant);
    }

    public async Task<Tenant> DeactivateTenantAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        context.Tenants.Update(tenant);
        return await Task.FromResult(tenant);
    }
}
