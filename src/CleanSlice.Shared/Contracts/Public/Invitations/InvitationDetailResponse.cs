namespace CleanSlice.Shared.Contracts.Public.Invitations;

public sealed record InvitationDetailResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string TenantName { get; init; } = string.Empty;
    public string TenantDomain { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string InvitedByName { get; init; } = string.Empty;
    public string? Message { get; init; }
    public DateTime ExpiresAt { get; init; }
    public bool IsExpired { get; init; }
    public bool IsUsed { get; init; }
}
