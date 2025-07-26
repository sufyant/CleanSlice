using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record RoleCreatedDomainEvent(
    Guid RoleId, 
    Guid TenantId, 
    string RoleName) : IDomainEvent;
