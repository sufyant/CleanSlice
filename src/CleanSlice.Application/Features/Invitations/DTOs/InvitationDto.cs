namespace CleanSlice.Application.Features.Invitations.DTOs;

public sealed record InvitationDto(
    Guid Id,
    string Email,
    Guid TenantId,
    string RoleName,
    string Token,
    Guid InvitedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt,
    bool IsUsed,
    DateTimeOffset? UsedAt,
    Guid? UsedBy);
