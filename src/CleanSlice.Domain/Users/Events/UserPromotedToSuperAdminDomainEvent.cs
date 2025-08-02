using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Users.Events;

public sealed record UserPromotedToSuperAdminDomainEvent(
    Guid UserId,
    Guid PromotedBy,
    DateTimeOffset PromotedAt) : IDomainEvent;
