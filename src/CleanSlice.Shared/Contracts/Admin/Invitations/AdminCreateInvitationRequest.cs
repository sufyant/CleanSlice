using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Admin.Invitations;

public sealed record AdminCreateInvitationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public Guid TenantId { get; init; }

    [Required]
    public Guid RoleId { get; init; }

    public int ExpirationDays { get; init; } = 7; // Default 7 days

    [StringLength(500)]
    public string? Message { get; init; } // Optional invitation message
}
