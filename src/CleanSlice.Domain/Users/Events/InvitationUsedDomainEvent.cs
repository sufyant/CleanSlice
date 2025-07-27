using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record InvitationUsedDomainEvent(
    Guid InvitationId,
    Guid TenantId,
    string Email,
    Guid UsedBy) : IDomainEvent;
