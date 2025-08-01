using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Tenants;
using CleanSlice.Domain.Users.Events;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Users;

public sealed class UserTenant : AuditableEntity
{
    public Guid UserId { get; private set; }
    public Guid TenantId { get; private set; }
    public bool IsPrimary { get; private set; }
    public DateTimeOffset JoinedAt { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;
    public Tenant Tenant { get; private set; } = null!;

    private UserTenant() { }

    private UserTenant(Guid id, Guid userId, Guid tenantId, bool isPrimary)
    {
        Id = id;
        UserId = userId;
        TenantId = tenantId;
        IsPrimary = isPrimary;
        JoinedAt = DateTimeOffset.UtcNow;
    }

    public static UserTenant Create(Guid id, Guid userId, Guid tenantId, bool isPrimary = false)
    {
        var userTenant = new UserTenant(id, userId, tenantId, isPrimary);
        
        userTenant.RaiseDomainEvent(new UserJoinedTenantDomainEvent(userId, tenantId, isPrimary));
        
        return userTenant;
    }

    public void SetAsPrimary()
    {
        if (IsPrimary)
            return;

        IsPrimary = true;
        RaiseDomainEvent(new PrimaryTenantChangedDomainEvent(UserId, TenantId));
    }

    public void RemoveAsPrimary()
    {
        if (!IsPrimary)
            return;

        IsPrimary = false;
    }


}
