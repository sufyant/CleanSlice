using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record InvitationCreatedDomainEvent(
    Guid InvitationId,
    Guid TenantId,
    string Email,
    Guid RoleId,
    Guid InvitedBy,
    string Token,
    DateTimeOffset ExpiresAt) : IDomainEvent;
