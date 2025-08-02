namespace CleanSlice.Shared.Contracts.Tenant.Roles;

public sealed record TenantRoleDetailResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int UserCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }
    public RolePermissionInfo[] Permissions { get; init; } = [];
    public RoleUserInfo[] Users { get; init; } = [];
}

public sealed record RolePermissionInfo
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime AssignedAt { get; init; }
}

public sealed record RoleUserInfo
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public DateTime AssignedAt { get; init; }
}
