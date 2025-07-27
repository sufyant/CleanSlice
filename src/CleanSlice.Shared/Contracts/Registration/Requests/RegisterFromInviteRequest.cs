using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Registration.Requests;

public sealed record RegisterFromInviteRequest(
    [Required] 
    string Token,
    
    [Required] 
    [EmailAddress] 
    string Email,
    
    [Required] 
    [StringLength(100)] 
    string FirstName,
    
    [Required] 
    [StringLength(100)] 
    string LastName
    );
