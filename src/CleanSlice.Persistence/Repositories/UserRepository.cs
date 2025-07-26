using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdentityId == identityId, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Where(u => u.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetWithRolesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersWithRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Email.Value == email, cancellationToken);
    }

    public async Task<bool> ExistsByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.IdentityId == identityId, cancellationToken);
    }
}
