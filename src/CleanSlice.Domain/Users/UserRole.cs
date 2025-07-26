using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Users;

public sealed class UserRole : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }
    public DateTimeOffset AssignedAt { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;
    public Role Role { get; private set; } = null!;

    private UserRole() { }

    private UserRole(Guid id, Guid userId, Guid roleId)
    {
        Id = id;
        UserId = userId;
        RoleId = roleId;
        AssignedAt = DateTimeOffset.UtcNow;
    }

    public static UserRole Create(Guid id, Guid userId, Guid roleId)
    {
        return new UserRole(id, userId, roleId);
    }
}
