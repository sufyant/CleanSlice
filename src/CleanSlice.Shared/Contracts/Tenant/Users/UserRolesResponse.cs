namespace CleanSlice.Shared.Contracts.Tenant.Users;

public sealed record UserRolesResponse
{
    public Guid UserId { get; init; }
    public string UserEmail { get; init; } = string.Empty;
    public UserRoleDetail[] Roles { get; init; } = [];
    public string[] AllPermissions { get; init; } = []; // Aggregated from all roles
}

public sealed record UserRoleDetail
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string[] Permissions { get; init; } = [];
    public DateTime AssignedAt { get; init; }
}
