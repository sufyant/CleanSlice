namespace CleanSlice.Shared.Contracts.Auth;

public sealed record LoginResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public UserInfo User { get; init; } = null!;
    public TenantInfo[] Tenants { get; init; } = [];
    public Guid? CurrentTenantId { get; init; }
}

public sealed record UserInfo
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public bool IsSuperAdmin { get; init; }
}

public sealed record TenantInfo
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public bool IsPrimary { get; init; }
    public string[] Roles { get; init; } = [];
}
