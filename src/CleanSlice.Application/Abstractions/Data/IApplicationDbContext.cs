using CleanSlice.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserTenant> UserTenants { get; }
    DbSet<Role> Roles { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<Invitation> Invitations { get; }
}
