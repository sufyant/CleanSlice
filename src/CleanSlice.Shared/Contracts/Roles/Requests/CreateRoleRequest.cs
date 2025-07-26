using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Roles.Requests;

public sealed record CreateRoleRequest(
    [Required]
    [StringLength(100, MinimumLength = 1)]
    string Name,
    
    [Required]
    [StringLength(500, MinimumLength = 1)]
    string Description,
    
    IEnumerable<string> Permissions
    );
