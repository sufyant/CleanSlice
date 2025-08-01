using CleanSlice.Domain.Common.ValueObjects;
using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserDeactivatedDomainEvent(
    Guid UserId,
    ExternalIdentityId ExternalIdentityId) : IDomainEvent;
