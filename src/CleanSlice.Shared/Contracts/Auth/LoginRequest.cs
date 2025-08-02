using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Auth;

public sealed record LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; init; } = string.Empty;

    public bool RememberMe { get; init; } = false;
}
