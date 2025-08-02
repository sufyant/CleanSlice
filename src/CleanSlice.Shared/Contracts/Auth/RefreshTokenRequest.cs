using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Auth;

public sealed record RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}
