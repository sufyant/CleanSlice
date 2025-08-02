using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserPasswordRemovedDomainEvent(
    Guid UserId) : IDomainEvent;
