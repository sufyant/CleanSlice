namespace CleanSlice.Shared.Contracts.Tenant.Permissions;

public sealed record PermissionsByCategoryResponse
{
    public PermissionCategory[] Categories { get; init; } = [];
}

public sealed record PermissionCategory
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public PermissionSummary[] Permissions { get; init; } = [];
}

public sealed record PermissionSummary
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int RoleCount { get; init; }
}
