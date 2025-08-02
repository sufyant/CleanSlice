using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Users.Services;

public sealed class UserTenantDomainService
{
    public void ValidateUserCanJoinTenant(User user, Guid tenantId)
    {
        if (!user.IsActive)
            throw new DomainInvariantViolationException("Inactive user cannot join tenant");

        if (user.IsMemberOfTenant(tenantId))
            throw new BusinessRuleViolationException("User is already a member of this tenant");

        // Business rule: User can be member of maximum 10 tenants
        if (user.UserTenants.Count >= 10)
            throw new BusinessRuleViolationException("User cannot be member of more than 10 tenants");
    }

    public void ValidateUserCanLeaveTenant(User user, Guid tenantId)
    {
        if (!user.IsMemberOfTenant(tenantId))
            throw new BusinessRuleViolationException("User is not a member of this tenant");

        // Business rule: User must have at least one tenant
        if (user.UserTenants.Count <= 1)
            throw new BusinessRuleViolationException("User must be member of at least one tenant");
    }

    public void ValidatePrimaryTenantChange(User user, Guid newPrimaryTenantId)
    {
        if (!user.IsMemberOfTenant(newPrimaryTenantId))
            throw new BusinessRuleViolationException("Cannot set primary tenant - user is not a member of this tenant");

        var currentPrimary = user.GetPrimaryTenantId();
        if (currentPrimary == newPrimaryTenantId)
            throw new BusinessRuleViolationException("This tenant is already the primary tenant");
    }

    public void ValidateRoleAssignment(User user, Role role, Guid tenantId)
    {
        if (!user.IsActive)
            throw new DomainInvariantViolationException("Cannot assign role to inactive user");

        if (!user.IsMemberOfTenant(tenantId))
            throw new BusinessRuleViolationException("User must be a member of the tenant to receive roles");

        if (role.TenantId != tenantId)
            throw new DomainInvariantViolationException("Role does not belong to the specified tenant");

        if (!role.IsActive)
            throw new DomainInvariantViolationException("Cannot assign inactive role");

        if (user.HasRoleInTenant(role.Id, tenantId))
            throw new BusinessRuleViolationException("User already has this role in the tenant");

        // Business rule: User can have maximum 5 roles per tenant
        var userRolesInTenant = user.GetRolesInTenant(tenantId).Count();
        if (userRolesInTenant >= 5)
            throw new BusinessRuleViolationException("User cannot have more than 5 roles per tenant");
    }
}
