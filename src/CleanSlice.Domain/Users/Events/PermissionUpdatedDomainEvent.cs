using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record PermissionUpdatedDomainEvent(
    Guid PermissionId, 
    string OldPermissionName, 
    string NewPermissionName) : IDomainEvent;
