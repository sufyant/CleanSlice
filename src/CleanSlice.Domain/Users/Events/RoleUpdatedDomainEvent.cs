using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record RoleUpdatedDomainEvent(
    Guid RoleId, 
    Guid TenantId, 
    string OldRoleName, 
    string NewRoleName) : IDomainEvent;
