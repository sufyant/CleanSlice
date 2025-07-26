using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserUpdatedDomainEvent(
    Guid UserId, 
    Guid TenantId, 
    string IdentityId, 
    string OldEmail, 
    string NewEmail) : IDomainEvent;
