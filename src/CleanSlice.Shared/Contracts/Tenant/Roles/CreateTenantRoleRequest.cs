using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Tenant.Roles;

public sealed record CreateTenantRoleRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [StringLength(200)]
    public string? Description { get; init; }

    public Guid[] PermissionIds { get; init; } = []; // Initial permissions
}

public sealed record CreateTenantRoleResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

public sealed record UpdateTenantRoleRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [StringLength(200)]
    public string? Description { get; init; }
}

public sealed record UpdateTenantRoleResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime LastModifiedAt { get; init; }
}
