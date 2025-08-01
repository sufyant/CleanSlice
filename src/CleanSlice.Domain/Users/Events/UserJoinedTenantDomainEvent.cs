using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserJoinedTenantDomainEvent(
    Guid UserId,
    Guid TenantId,
    bool IsPrimary) : IDomainEvent;
