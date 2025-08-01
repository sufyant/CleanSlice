using CleanSlice.Domain.Common.ValueObjects;
using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(
    Guid UserId,
    ExternalIdentityId ExternalIdentityId,
    Email Email) : IDomainEvent;
