using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Users.Events;
using CleanSlice.Domain.Users.ValueObjects;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Users;

public sealed class Role : AuditableTenantEntityWithSoftDelete
{
    public RoleName Name { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public bool IsSystemRole { get; private set; }

    // Navigation properties
    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private readonly List<RolePermission> _rolePermissions = [];
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private Role() { }

    private Role(Guid id, Guid tenantId, RoleName name, string description, bool isSystemRole)
    {
        Id = id;
        TenantId = tenantId;
        Name = name;
        Description = description;
        IsSystemRole = isSystemRole;
    }

    public static Role Create(Guid id, Guid tenantId, string name, string description, bool isSystemRole = false)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException(nameof(description), "Description cannot be empty");

        var roleName = RoleName.Create(name);
        var role = new Role(id, tenantId, roleName, description.Trim(), isSystemRole);

        role.RaiseDomainEvent(new RoleCreatedDomainEvent(id, tenantId, name));

        return role;
    }

    public void Update(string name, string description)
    {
        if (IsSystemRole)
            throw new BusinessRuleViolationException("Cannot update system role");

        if (!IsActive)
            throw new DomainInvariantViolationException("Cannot update deleted role");

        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException(nameof(description), "Description cannot be empty");

        var oldName = Name.Value;
        Name = RoleName.Create(name);
        Description = description.Trim();

        RaiseDomainEvent(new RoleUpdatedDomainEvent(Id, TenantId, oldName, name));
    }

    public void AssignPermission(Permission permission)
    {
        if (!IsActive)
            throw new DomainInvariantViolationException("Cannot assign permission to deleted role");

        if (_rolePermissions.Any(rp => rp.PermissionId == permission.Id))
            return; // Already assigned

        // Business rule: System roles cannot have permissions modified
        if (IsSystemRole)
            throw new BusinessRuleViolationException("Cannot modify permissions of system roles");

        var rolePermission = RolePermission.Create(Guid.NewGuid(), Id, permission.Id);
        _rolePermissions.Add(rolePermission);

        RaiseDomainEvent(new RolePermissionAssignedDomainEvent(Id, permission.Id, TenantId));
    }

    public void RemovePermission(Guid permissionId)
    {
        if (!IsActive)
            throw new DomainInvariantViolationException("Cannot remove permission from deleted role");

        // Business rule: System roles cannot have permissions modified
        if (IsSystemRole)
            throw new BusinessRuleViolationException("Cannot modify permissions of system roles");

        var rolePermission = _rolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
        if (rolePermission == null)
            return; // Not assigned

        _rolePermissions.Remove(rolePermission);

        RaiseDomainEvent(new RolePermissionRemovedDomainEvent(Id, permissionId, TenantId));
    }

    public bool HasPermission(Guid permissionId)
    {
        return _rolePermissions.Any(rp => rp.PermissionId == permissionId);
    }

    public IEnumerable<Guid> GetPermissionIds()
    {
        return _rolePermissions.Select(rp => rp.PermissionId);
    }
}
