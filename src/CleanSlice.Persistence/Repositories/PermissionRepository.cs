using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class PermissionRepository(ApplicationDbContext dbContext) : BaseRepository<Permission>(dbContext), IPermissionRepository
{
    public async Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name.Value.Equals(name, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .Where(p => p.Category == category)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .AnyAsync(p => p.Name.Value.Equals(name, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
    }

    // Pagination methods
    public async Task<PagedResult<Permission>> GetPagedPermissionsAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Permissions.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p =>
                p.Name.Value.Contains(request.SearchTerm) ||
                p.Description.Contains(request.SearchTerm) ||
                p.Category.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderBy(p => p.Name.Value);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<PagedResult<Permission>> GetPagedPermissionsByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Permissions
            .AsNoTracking()
            .Where(p => p.Category == category);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p =>
                p.Name.Value.Contains(request.SearchTerm) ||
                p.Description.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderBy(p => p.Name.Value);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
