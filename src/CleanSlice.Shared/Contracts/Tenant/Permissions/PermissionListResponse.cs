namespace CleanSlice.Shared.Contracts.Tenant.Permissions;

public sealed record PermissionListResponse
{
    public PermissionSummary[] Permissions { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

public sealed record PermissionSummary
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int RoleCount { get; init; } // How many roles have this permission
}
