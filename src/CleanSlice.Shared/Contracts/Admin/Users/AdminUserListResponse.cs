namespace CleanSlice.Shared.Contracts.Admin.Users;

public sealed record AdminUserListResponse
{
    public AdminUserSummary[] Users { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

public sealed record AdminUserSummary
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public bool IsSuperAdmin { get; init; }
    public int TenantCount { get; init; }
    public string[] TenantNames { get; init; } = [];
    public DateTime? LastLogin { get; init; }
    public DateTime CreatedAt { get; init; }
}
