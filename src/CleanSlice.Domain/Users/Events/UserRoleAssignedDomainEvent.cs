using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserRoleAssignedDomainEvent(
    Guid UserId, 
    Guid RoleId, 
    Guid TenantId) : IDomainEvent;
