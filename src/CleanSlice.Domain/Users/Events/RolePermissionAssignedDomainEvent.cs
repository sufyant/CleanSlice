using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record RolePermissionAssignedDomainEvent(
    Guid RoleId, 
    Guid PermissionId, 
    Guid TenantId) : IDomainEvent;
