using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(
    Guid UserId, 
    Guid TenantId, 
    string IdentityId, 
    string Email) : IDomainEvent;
