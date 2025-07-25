using CleanSlice.Application.Abstractions.Repositories.Management;
using CleanSlice.Domain.Tenants;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.TenantManagement.Repositories;

public sealed class TenantManagementRepository(TenantCatalogDbContext context) : ITenantManagementRepository
{
    public async Task<PagedResult<Tenant>> GetPagedTenantsAsync(int page, int pageSize, string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        // Build the query
        var query = context.Tenants.AsNoTracking();

        // Apply search filter if searchTerm is provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t =>
                t.Name.Contains(searchTerm) ||
                t.Slug.Contains(searchTerm) ||
                t.Domain.Contains(searchTerm)
            );
        }

        // Get total count for pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Calculate pagination values
        var skip = (page - 1) * pageSize;
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Get paged data
        var items = await query
            .OrderBy(t => t.Name) // Default ordering
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Return paged result
        return PagedResult<Tenant>.Create(items, totalCount, page, pageSize);
    }

    public async Task<IReadOnlyList<Tenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default) =>
        await context.Tenants.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Tenants.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await context.Tenants.AnyAsync(t => t.Name == name, cancellationToken);

    public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default) =>
        await context.Tenants.AnyAsync(t => t.Slug == slug, cancellationToken);

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
