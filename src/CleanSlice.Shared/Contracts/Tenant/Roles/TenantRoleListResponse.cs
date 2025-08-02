namespace CleanSlice.Shared.Contracts.Tenant.Roles;

public sealed record TenantRoleListResponse
{
    public TenantRoleSummary[] Roles { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

public sealed record TenantRoleSummary
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int UserCount { get; init; }
    public int PermissionCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }
}
