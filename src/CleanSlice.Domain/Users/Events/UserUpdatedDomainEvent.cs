using CleanSlice.Domain.Common.ValueObjects;
using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserUpdatedDomainEvent(
    Guid UserId,
    ExternalIdentityId ExternalIdentityId,
    Email OldEmail,
    Email NewEmail) : IDomainEvent;
