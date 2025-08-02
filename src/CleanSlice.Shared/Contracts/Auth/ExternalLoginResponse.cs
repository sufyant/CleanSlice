namespace CleanSlice.Shared.Contracts.Auth;

public sealed record ExternalLoginResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public UserInfo User { get; init; } = null!;
    public TenantInfo[] Tenants { get; init; } = [];
    public Guid? CurrentTenantId { get; init; }
    public bool IsNewUser { get; init; } // First time login
}
