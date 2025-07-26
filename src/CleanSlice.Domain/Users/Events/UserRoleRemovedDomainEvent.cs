using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserRoleRemovedDomainEvent(
    Guid UserId, 
    Guid RoleId, 
    Guid TenantId) : IDomainEvent;
