using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserUpdatedDomainEvent(
    Guid UserId,
    string ExternalIdentityId,
    string OldEmail,
    string NewEmail) : IDomainEvent;
