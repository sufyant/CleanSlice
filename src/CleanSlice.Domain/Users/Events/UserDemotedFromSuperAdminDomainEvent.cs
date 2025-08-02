using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserDemotedFromSuperAdminDomainEvent(
    Guid UserId,
    Guid DemotedBy,
    DateTimeOffset DemotedAt) : IDomainEvent;
