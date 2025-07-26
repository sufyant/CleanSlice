using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Users.Events;
using CleanSlice.Domain.Users.ValueObjects;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Users;

public sealed class Permission : AuditableEntity
{
    public PermissionName Name { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;

    // Navigation properties
    private readonly List<RolePermission> _rolePermissions = [];
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private Permission() { }

    private Permission(Guid id, PermissionName name, string description, string category)
    {
        Id = id;
        Name = name;
        Description = description;
        Category = category;
    }

    public static Permission Create(Guid id, string name, string description, string category)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException(nameof(description), "Description cannot be empty");

        if (string.IsNullOrWhiteSpace(category))
            throw new ValidationException(nameof(category), "Category cannot be empty");

        var permissionName = PermissionName.Create(name);
        var permission = new Permission(id, permissionName, description.Trim(), category.Trim());

        permission.RaiseDomainEvent(new PermissionCreatedDomainEvent(id, name));

        return permission;
    }

    public void Update(string name, string description, string category)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException(nameof(description), "Description cannot be empty");

        if (string.IsNullOrWhiteSpace(category))
            throw new ValidationException(nameof(category), "Category cannot be empty");

        var oldName = Name.Value;
        Name = PermissionName.Create(name);
        Description = description.Trim();
        Category = category.Trim();

        RaiseDomainEvent(new PermissionUpdatedDomainEvent(Id, oldName, name));
    }
}
