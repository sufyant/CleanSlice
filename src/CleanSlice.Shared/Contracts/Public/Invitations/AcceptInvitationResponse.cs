namespace CleanSlice.Shared.Contracts.Public.Invitations;

public sealed record AcceptInvitationResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public UserInfo User { get; init; } = null!;
    public TenantInfo Tenant { get; init; } = null!;
    public string[] AssignedRoles { get; init; } = [];
}

public sealed record UserInfo
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
}

public sealed record TenantInfo
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Domain { get; init; } = string.Empty;
}
