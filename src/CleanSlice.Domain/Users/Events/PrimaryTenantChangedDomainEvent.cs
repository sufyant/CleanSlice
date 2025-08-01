using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record PrimaryTenantChangedDomainEvent(
    Guid UserId,
    Guid NewPrimaryTenantId) : IDomainEvent;
