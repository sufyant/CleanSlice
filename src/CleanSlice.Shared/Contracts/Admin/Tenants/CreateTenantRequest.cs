using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Admin.Tenants;

public sealed record CreateTenantRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Domain { get; init; } = string.Empty;

    [StringLength(50, MinimumLength = 2)]
    public string? Slug { get; init; } // Auto-generated if not provided

    [StringLength(500)]
    public string? Description { get; init; }

    public string? ConnectionString { get; init; } // Optional custom connection string
}
