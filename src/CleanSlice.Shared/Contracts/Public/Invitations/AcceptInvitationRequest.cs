using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Public.Invitations;

public sealed record AcceptInvitationRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; init; } = string.Empty;

    // For local authentication
    [MinLength(6)]
    public string? Password { get; init; }

    [Compare(nameof(Password))]
    public string? ConfirmPassword { get; init; }

    // For external authentication
    public string? ExternalAccessToken { get; init; }
    public string? ExternalProvider { get; init; } // "Google" or "Microsoft"
}
