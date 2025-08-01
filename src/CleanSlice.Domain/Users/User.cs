using CleanSlice.Domain.Common.Enums;
using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Common.ValueObjects;
using CleanSlice.Domain.Users.Events;
using CleanSlice.Domain.Users.ValueObjects;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Users;

public sealed class User : AuditableEntityWithSoftDelete
{
    public ExternalIdentityId ExternalIdentityId { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public FullName FullName { get; private set; } = null!;
    public DateTimeOffset? LastLogin { get; private set; }

    // Navigation properties - User can belong to multiple tenants
    private readonly List<UserTenant> _userTenants = [];
    public IReadOnlyCollection<UserTenant> UserTenants => _userTenants.AsReadOnly();

    // User roles across all tenants
    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    // Convenience properties
    public string FirstName => FullName.FirstName;
    public string LastName => FullName.LastName;
    public string DisplayName => FullName.ToString();
    public string Initials => FullName.GetInitials();

    private User() { }

    private User(Guid id, ExternalIdentityId externalIdentityId, Email email, FullName fullName)
    {
        Id = id;
        ExternalIdentityId = externalIdentityId;
        Email = email;
        FullName = fullName;
    }

    public static User Create(Guid id, string externalIdentityId, string email, string firstName, string lastName, LoginProvider provider = LoginProvider.Local)
    {
        if (string.IsNullOrWhiteSpace(externalIdentityId))
            throw new ValidationException(nameof(externalIdentityId), "External identity ID cannot be empty");

        var userEmail = Email.Create(email);
        var externalId = ExternalIdentityId.Create(externalIdentityId.Trim(), provider);
        var fullName = FullName.Create(firstName, lastName);
        var user = new User(id, externalId, userEmail, fullName);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(id, externalId, userEmail));

        return user;
    }

    // Helper method for creating user with initial tenant
    public static User CreateWithTenant(Guid id, string externalIdentityId, string email, string firstName, string lastName, Guid tenantId, LoginProvider provider = LoginProvider.Local)
    {
        var user = Create(id, externalIdentityId, email, firstName, lastName, provider);
        user.JoinTenant(tenantId, isPrimary: true);
        return user;
    }

    public void Update(string email, string firstName, string lastName)
    {
        if (!IsActive)
            throw new BusinessRuleViolationException("Cannot update deleted user");

        var oldEmail = Email.Value;
        Email = Email.Create(email);
        FullName = FullName.Create(firstName, lastName);

        var oldEmailValue = Email.Create(oldEmail);
        RaiseDomainEvent(new UserUpdatedDomainEvent(Id, ExternalIdentityId, oldEmailValue, Email));
    }

    public void UpdateLastLogin()
    {
        if (!IsActive)
            throw new BusinessRuleViolationException("Cannot update last login for deleted user");

        LastLogin = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new UserLastLoginUpdatedDomainEvent(Id, LastLogin.Value));
    }

    public void Activate()
    {
        if (IsActive)
            return; // Already active

        DeletedAt = null;
        DeletedBy = null;
        RaiseDomainEvent(new UserActivatedDomainEvent(Id, ExternalIdentityId));
    }

    public void Deactivate(Guid deletedBy)
    {
        if (!IsActive)
            return; // Already deactivated

        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = deletedBy;
        RaiseDomainEvent(new UserDeactivatedDomainEvent(Id, ExternalIdentityId));
    }

    // Tenant Management Methods
    public void JoinTenant(Guid tenantId, bool isPrimary = false)
    {
        // Business rule validation
        var domainService = new Services.UserTenantDomainService();
        domainService.ValidateUserCanJoinTenant(this, tenantId);

        if (_userTenants.Any(ut => ut.TenantId == tenantId))
            return; // Already a member

        // If this is the first tenant or explicitly set as primary, make it primary
        if (!_userTenants.Any() || isPrimary)
        {
            // Remove primary status from other tenants
            foreach (var existingTenant in _userTenants.Where(ut => ut.IsPrimary))
            {
                existingTenant.RemoveAsPrimary();
            }
            isPrimary = true;
        }

        var userTenant = UserTenant.Create(Guid.NewGuid(), Id, tenantId, isPrimary);
        _userTenants.Add(userTenant);
    }

    public void LeaveTenant(Guid tenantId)
    {
        // Business rule validation
        var domainService = new Services.UserTenantDomainService();
        domainService.ValidateUserCanLeaveTenant(this, tenantId);

        var userTenant = _userTenants.FirstOrDefault(ut => ut.TenantId == tenantId);
        if (userTenant == null)
            return; // Not a member

        _userTenants.Remove(userTenant);
        RaiseDomainEvent(new UserLeftTenantDomainEvent(Id, tenantId));

        // If this was the primary tenant, set another as primary
        if (userTenant.IsPrimary && _userTenants.Any())
        {
            var newPrimary = _userTenants.First();
            newPrimary.SetAsPrimary();
        }
    }

    public void SetPrimaryTenant(Guid tenantId)
    {
        // Business rule validation
        var domainService = new Services.UserTenantDomainService();
        domainService.ValidatePrimaryTenantChange(this, tenantId);

        var targetTenant = _userTenants.FirstOrDefault(ut => ut.TenantId == tenantId);
        if (targetTenant == null)
            throw new BusinessRuleViolationException("User is not a member of this tenant");

        if (targetTenant.IsPrimary)
            return; // Already primary

        // Remove primary status from other tenants
        foreach (var existingTenant in _userTenants.Where(ut => ut.IsPrimary))
        {
            existingTenant.RemoveAsPrimary();
        }

        targetTenant.SetAsPrimary();
    }

    public Guid? GetPrimaryTenantId()
    {
        return _userTenants.FirstOrDefault(ut => ut.IsPrimary)?.TenantId;
    }

    public IEnumerable<Guid> GetTenantIds()
    {
        return _userTenants.Select(ut => ut.TenantId);
    }

    public bool IsMemberOfTenant(Guid tenantId)
    {
        return _userTenants.Any(ut => ut.TenantId == tenantId);
    }

    public bool HasMultipleTenants()
    {
        return _userTenants.Count > 1;
    }

    public bool IsNewUser()
    {
        return !LastLogin.HasValue;
    }

    // Role Management Methods (for specific tenant)
    public void AssignRoleInTenant(Role role, Guid tenantId)
    {
        // Business rule validation
        var domainService = new Services.UserTenantDomainService();
        domainService.ValidateRoleAssignment(this, role, tenantId);

        if (_userRoles.Any(ur => ur.RoleId == role.Id && ur.TenantId == tenantId))
            return; // Already assigned

        var userRole = UserRole.Create(Guid.NewGuid(), Id, role.Id, tenantId);
        _userRoles.Add(userRole);

        RaiseDomainEvent(new UserRoleAssignedDomainEvent(Id, role.Id, tenantId));
    }

    public void RemoveRoleInTenant(Guid roleId, Guid tenantId)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId && ur.TenantId == tenantId);
        if (userRole == null)
            return; // Not assigned

        _userRoles.Remove(userRole);

        RaiseDomainEvent(new UserRoleRemovedDomainEvent(Id, roleId, tenantId));
    }

    public bool HasRoleInTenant(Guid roleId, Guid tenantId)
    {
        return _userRoles.Any(ur => ur.RoleId == roleId && ur.TenantId == tenantId);
    }

    public IEnumerable<Guid> GetRoleIdsInTenant(Guid tenantId)
    {
        return _userRoles.Where(ur => ur.TenantId == tenantId).Select(ur => ur.RoleId);
    }

    public IEnumerable<UserRole> GetRolesInTenant(Guid tenantId)
    {
        return _userRoles.Where(ur => ur.TenantId == tenantId);
    }

    public bool HasAnyRoleInTenant(Guid tenantId)
    {
        return _userRoles.Any(ur => ur.TenantId == tenantId);
    }
}
