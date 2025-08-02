namespace CleanSlice.Shared.Contracts.Tenant.Permissions;

public sealed record PermissionDetailResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public PermissionRoleInfo[] Roles { get; init; } = []; // Which roles have this permission
}

public sealed record PermissionRoleInfo
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int UserCount { get; init; }
    public DateTime AssignedAt { get; init; }
}
