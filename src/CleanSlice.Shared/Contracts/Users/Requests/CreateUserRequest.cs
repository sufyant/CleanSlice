using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Users.Requests;

public sealed record CreateUserRequest(
    [Required]
    [EmailAddress]
    string Email,
    
    [Required]
    [StringLength(100, MinimumLength = 1)]
    string FirstName,
    
    [Required]
    [StringLength(100, MinimumLength = 1)]
    string LastName,
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    string Password,
    
    IEnumerable<string> Roles);
