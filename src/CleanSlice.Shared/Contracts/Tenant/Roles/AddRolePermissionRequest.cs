using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Tenant.Roles;

public sealed record AddRolePermissionRequest
{
    [Required]
    public Guid PermissionId { get; init; }
}
