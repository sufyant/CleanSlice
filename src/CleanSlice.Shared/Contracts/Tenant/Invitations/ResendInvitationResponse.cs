namespace CleanSlice.Shared.Contracts.Tenant.Invitations;

public sealed record ResendInvitationResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public DateTime NewExpiresAt { get; init; }
    public DateTime ResentAt { get; init; }
}
