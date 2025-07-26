using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Users;

public sealed class RolePermission : BaseEntity
{
    public Guid RoleId { get; private set; }
    public Guid PermissionId { get; private set; }
    public DateTimeOffset AssignedAt { get; private set; }

    // Navigation properties
    public Role Role { get; private set; } = null!;
    public Permission Permission { get; private set; } = null!;

    private RolePermission() { }

    private RolePermission(Guid id, Guid roleId, Guid permissionId)
    {
        Id = id;
        RoleId = roleId;
        PermissionId = permissionId;
        AssignedAt = DateTimeOffset.UtcNow;
    }

    public static RolePermission Create(Guid id, Guid roleId, Guid permissionId)
    {
        return new RolePermission(id, roleId, permissionId);
    }
}
