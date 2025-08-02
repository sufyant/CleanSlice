namespace CleanSlice.Shared.Contracts.Tenant.Invitations;

public sealed record TenantInvitationDetailResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string InvitedByEmail { get; init; } = string.Empty;
    public string InvitedByName { get; init; } = string.Empty;
    public string? Message { get; init; }
    public bool IsUsed { get; init; }
    public bool IsExpired { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ExpiresAt { get; init; }
    public DateTime? UsedAt { get; init; }
    public string? UsedByEmail { get; init; }
}
