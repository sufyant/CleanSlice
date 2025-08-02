namespace CleanSlice.Shared.Contracts.Tenant.Permissions;

public sealed record PermissionListRequest
{
    public string? SearchTerm { get; init; }
    public string? Category { get; init; } // Filter by category
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50; // Higher default for permissions
    public string? SortBy { get; init; } = "Name";
    public bool SortDescending { get; init; } = false;
}
