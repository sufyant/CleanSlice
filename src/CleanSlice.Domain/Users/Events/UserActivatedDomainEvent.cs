using CleanSlice.Domain.Common.ValueObjects;
using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserActivatedDomainEvent(
    Guid UserId,
    ExternalIdentityId ExternalIdentityId) : IDomainEvent;
