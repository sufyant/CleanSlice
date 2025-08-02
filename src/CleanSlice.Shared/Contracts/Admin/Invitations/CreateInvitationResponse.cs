namespace CleanSlice.Shared.Contracts.Admin.Invitations;

public sealed record CreateInvitationResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string TenantName { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; }
}
