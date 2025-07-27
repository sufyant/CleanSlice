using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record InvitationExtendedDomainEvent(
    Guid InvitationId,
    Guid TenantId,
    string Email,
    DateTimeOffset NewExpirationDate) : IDomainEvent;
