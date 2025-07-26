using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record RolePermissionRemovedDomainEvent(
    Guid RoleId, 
    Guid PermissionId, 
    Guid TenantId) : IDomainEvent;
