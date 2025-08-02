namespace CleanSlice.Shared.Contracts.Tenant.Roles;

public sealed record TenantRoleListRequest
{
    public string? SearchTerm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; } = "Name";
    public bool SortDescending { get; init; } = false;
    public bool IncludeUserCount { get; init; } = true;
}
