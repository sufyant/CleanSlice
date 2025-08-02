namespace CleanSlice.Shared.Contracts.Tenant.Users;

public sealed record TenantUserDetailResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public string AuthProvider { get; init; } = string.Empty;
    public DateTime? LastLogin { get; init; }
    public DateTime JoinedAt { get; init; }
    public UserRoleInfo[] Roles { get; init; } = [];
    public string[] Permissions { get; init; } = [];
}

public sealed record UserRoleInfo
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime AssignedAt { get; init; }
}
