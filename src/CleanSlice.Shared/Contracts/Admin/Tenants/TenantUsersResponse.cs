namespace CleanSlice.Shared.Contracts.Admin.Tenants;

public sealed record TenantUsersResponse
{
    public TenantUserSummary[] Users { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

public sealed record TenantUserSummary
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public string[] Roles { get; init; } = [];
    public DateTime? LastLogin { get; init; }
    public DateTime JoinedAt { get; init; }
}
