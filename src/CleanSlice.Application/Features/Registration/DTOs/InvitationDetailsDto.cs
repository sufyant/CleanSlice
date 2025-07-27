namespace CleanSlice.Application.Features.Registration.DTOs;

public sealed record InvitationDetailsDto(
    Guid InvitationId,
    string Email,
    Guid TenantId,
    string RoleName,
    Guid InvitedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt);
