using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Auth;

public sealed record ExternalLoginRequest
{
    [Required]
    public string AccessToken { get; init; } = string.Empty;

    [Required]
    public string Provider { get; init; } = string.Empty; // "Google" or "Microsoft"

    public string? InvitationToken { get; init; } // Required for first-time users
}
