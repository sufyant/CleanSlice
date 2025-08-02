using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Users.Services;

public sealed class SuperAdminDomainService
{
    public void ValidateCanPromoteToSuperAdmin(User promotingUser, User targetUser)
    {
        // Security Rule 1: Only super admins can promote others
        if (!promotingUser.IsSuperAdmin)
            throw new BusinessRuleViolationException("Only super admins can promote users to super admin");

        // Security Rule 2: Promoting user must be active
        if (!promotingUser.IsActive)
            throw new DomainInvariantViolationException("Inactive user cannot promote others");

        // Security Rule 3: Target user must be active
        if (!targetUser.IsActive)
            throw new DomainInvariantViolationException("Cannot promote inactive user to super admin");

        // Security Rule 4: Target user cannot already be super admin
        if (targetUser.IsSuperAdmin)
            throw new BusinessRuleViolationException("User is already a super admin");

        // Security Rule 5: Cannot promote yourself (prevents privilege escalation)
        if (promotingUser.Id == targetUser.Id)
            throw new BusinessRuleViolationException("Cannot promote yourself to super admin");
    }

    public void ValidateCanDemoteFromSuperAdmin(User demotingUser, User targetUser)
    {
        // Security Rule 1: Only super admins can demote others
        if (!demotingUser.IsSuperAdmin)
            throw new BusinessRuleViolationException("Only super admins can demote super admins");

        // Security Rule 2: Demoting user must be active
        if (!demotingUser.IsActive)
            throw new DomainInvariantViolationException("Inactive user cannot demote others");

        // Security Rule 3: Target must be super admin
        if (!targetUser.IsSuperAdmin)
            throw new BusinessRuleViolationException("User is not a super admin");

        // Security Rule 4: Cannot demote yourself (prevents lockout)
        if (demotingUser.Id == targetUser.Id)
            throw new BusinessRuleViolationException("Cannot demote yourself from super admin");
    }

    public void ValidateCanCreateTenant(User user)
    {
        if (!user.IsSuperAdmin)
            throw new BusinessRuleViolationException("Only super admins can create tenants");

        if (!user.IsActive)
            throw new DomainInvariantViolationException("Inactive user cannot create tenants");
    }

    public void ValidateCanDeleteTenant(User user, Guid tenantId)
    {
        if (!user.IsSuperAdmin)
            throw new BusinessRuleViolationException("Only super admins can delete tenants");

        if (!user.IsActive)
            throw new DomainInvariantViolationException("Inactive user cannot delete tenants");
    }

    public void ValidateCanAccessTenantData(User user, Guid tenantId)
    {
        // Super admins can access any tenant data
        // Regular users only their member tenants
        if (!user.CanAccessTenant(tenantId))
            throw new BusinessRuleViolationException("User does not have access to this tenant");
    }

    public void ValidateCanManageUser(User managingUser, User targetUser)
    {
        if (!managingUser.IsSuperAdmin)
            throw new BusinessRuleViolationException("Only super admins can manage other users globally");

        if (!managingUser.IsActive)
            throw new DomainInvariantViolationException("Inactive user cannot manage others");

        // Security: Super admins cannot manage other super admins (except demotion)
        if (targetUser.IsSuperAdmin && managingUser.Id != targetUser.Id)
            throw new BusinessRuleViolationException("Super admins cannot manage other super admins");
    }
}
