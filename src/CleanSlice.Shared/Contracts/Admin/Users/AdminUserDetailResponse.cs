namespace CleanSlice.Shared.Contracts.Admin.Users;

public sealed record AdminUserDetailResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public bool IsSuperAdmin { get; init; }
    public string AuthProvider { get; init; } = string.Empty;
    public bool HasPassword { get; init; }
    public DateTime? LastLogin { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }
    public UserTenantMembership[] TenantMemberships { get; init; } = [];
}

public sealed record UserTenantMembership
{
    public Guid TenantId { get; init; }
    public string TenantName { get; init; } = string.Empty;
    public string TenantSlug { get; init; } = string.Empty;
    public bool IsPrimary { get; init; }
    public string[] Roles { get; init; } = [];
    public DateTime JoinedAt { get; init; }
}
