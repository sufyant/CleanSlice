using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserPasswordChangedDomainEvent(
    Guid UserId) : IDomainEvent;
