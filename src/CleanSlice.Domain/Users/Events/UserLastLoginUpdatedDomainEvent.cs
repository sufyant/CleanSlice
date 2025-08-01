using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserLastLoginUpdatedDomainEvent(
    Guid UserId,
    DateTimeOffset LastLogin) : IDomainEvent;
