using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserActivatedDomainEvent(
    Guid UserId, 
    Guid TenantId, 
    string IdentityId) : IDomainEvent;
