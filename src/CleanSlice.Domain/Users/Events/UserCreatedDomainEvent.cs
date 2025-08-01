using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(
    Guid UserId,
    string ExternalIdentityId,
    string Email) : IDomainEvent;
