using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserDeactivatedDomainEvent(
    Guid UserId,
    string ExternalIdentityId) : IDomainEvent;
