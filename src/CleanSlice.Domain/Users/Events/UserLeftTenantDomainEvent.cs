using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserLeftTenantDomainEvent(
    Guid UserId,
    Guid TenantId) : IDomainEvent;
