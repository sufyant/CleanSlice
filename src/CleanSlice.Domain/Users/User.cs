using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Common.ValueObjects;
using CleanSlice.Domain.Users.Events;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Users;

public sealed class User : AuditableTenantEntityWithSoftDelete
{
    public string IdentityId { get; private set; } = string.Empty; // Keycloak user ID
    public Email Email { get; private set; } = null!;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    // Navigation properties
    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public string FullName => $"{FirstName} {LastName}".Trim();

    private User() { }

    private User(Guid id, Guid tenantId, string identityId, Email email, string firstName, string lastName)
    {
        Id = id;
        TenantId = tenantId;
        IdentityId = identityId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public static User Create(Guid id, Guid tenantId, string identityId, string email, string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(identityId))
            throw new ValidationException(nameof(identityId), "Identity ID cannot be empty");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ValidationException(nameof(firstName), "First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException(nameof(lastName), "Last name cannot be empty");

        var userEmail = Email.Create(email);
        var user = new User(id, tenantId, identityId.Trim(), userEmail, firstName.Trim(), lastName.Trim());

        user.RaiseDomainEvent(new UserCreatedDomainEvent(id, tenantId, identityId, email));

        return user;
    }

    public void Update(string email, string firstName, string lastName)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot update deleted user");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ValidationException(nameof(firstName), "First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException(nameof(lastName), "Last name cannot be empty");

        var oldEmail = Email.Value;
        Email = Email.Create(email);
        FirstName = firstName.Trim();
        LastName = lastName.Trim();

        RaiseDomainEvent(new UserUpdatedDomainEvent(Id, TenantId, IdentityId, oldEmail, email));
    }

    public void Activate()
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot activate deleted user");

        if (IsActive)
            return;

        DeletedAt = null;
        DeletedBy = null;
        RaiseDomainEvent(new UserActivatedDomainEvent(Id, TenantId, IdentityId));
    }

    public void Deactivate(Guid deletedBy)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot deactivate deleted user");

        if (!IsActive)
            return;

        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = deletedBy;
        RaiseDomainEvent(new UserDeactivatedDomainEvent(Id, TenantId, IdentityId));
    }

    public void AssignRole(Role role)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot assign role to deleted user");

        if (role.TenantId != TenantId)
            throw new BusinessRuleViolationException("Cannot assign role from different tenant");

        if (_userRoles.Any(ur => ur.RoleId == role.Id))
            return; // Already assigned

        var userRole = UserRole.Create(Guid.NewGuid(), Id, role.Id);
        _userRoles.Add(userRole);

        RaiseDomainEvent(new UserRoleAssignedDomainEvent(Id, role.Id, TenantId));
    }

    public void RemoveRole(Guid roleId)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot remove role from deleted user");

        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId);
        if (userRole == null)
            return; // Not assigned

        _userRoles.Remove(userRole);

        RaiseDomainEvent(new UserRoleRemovedDomainEvent(Id, roleId, TenantId));
    }

    public bool HasRole(Guid roleId)
    {
        return _userRoles.Any(ur => ur.RoleId == roleId);
    }

    public IEnumerable<Guid> GetRoleIds()
    {
        return _userRoles.Select(ur => ur.RoleId);
    }
}
