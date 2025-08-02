namespace CleanSlice.Shared.Contracts.Tenant.Invitations;

public sealed record CreateTenantInvitationResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; }
}
