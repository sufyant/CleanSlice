using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Common.Enums;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Exceptions.Infrastructure;
using CleanSlice.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByExternalIdentityIdAsync(string externalIdentityId, LoginProvider provider = LoginProvider.Local, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.ExternalIdentityId.Value == externalIdentityId &&
                                    u.ExternalIdentityId.Provider == provider, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
    }

    public async Task<User> GetByExternalIdentityIdRequiredAsync(string externalIdentityId, LoginProvider provider = LoginProvider.Local, CancellationToken cancellationToken = default)
    {
        var user = await GetByExternalIdentityIdAsync(externalIdentityId, provider, cancellationToken);
        return user ?? throw new EntityNotFoundException($"User with external identity ID '{externalIdentityId}' and provider '{provider}' was not found");
    }

    public async Task<User> GetByEmailRequiredAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await GetByEmailAsync(email, cancellationToken);
        return user ?? throw new EntityNotFoundException($"User with email '{email}' was not found");
    }

    public async Task<IEnumerable<User>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId))
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetWithTenantsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.UserTenants)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetWithTenantsAndRolesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.UserTenants)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersWithRoleInTenantAsync(Guid roleId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId) &&
                       u.UserRoles.Any(ur => ur.RoleId == roleId && ur.TenantId == tenantId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Email.Value == email, cancellationToken);
    }

    public async Task<bool> ExistsByExternalIdentityIdAsync(string externalIdentityId, LoginProvider provider = LoginProvider.Local, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.ExternalIdentityId.Value == externalIdentityId &&
                          u.ExternalIdentityId.Provider == provider, cancellationToken);
    }

    // Pagination methods
    public async Task<PagedResult<User>> GetPagedUsersAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(u =>
                u.FullName.FirstName.Contains(request.SearchTerm) ||
                u.FullName.LastName.Contains(request.SearchTerm) ||
                u.Email.Value.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderBy(u => u.FullName.FirstName);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<PagedResult<User>> GetPagedUsersByTenantAsync(Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId));

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(u =>
                u.FullName.FirstName.Contains(request.SearchTerm) ||
                u.FullName.LastName.Contains(request.SearchTerm) ||
                u.Email.Value.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderBy(u => u.FullName.FirstName);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<PagedResult<User>> GetPagedUsersWithRoleAsync(Guid roleId, Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId) &&
                       u.UserRoles.Any(ur => ur.RoleId == roleId && ur.TenantId == tenantId));

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(u =>
                u.FullName.FirstName.Contains(request.SearchTerm) ||
                u.FullName.LastName.Contains(request.SearchTerm) ||
                u.Email.Value.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderBy(u => u.FullName.FirstName);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
