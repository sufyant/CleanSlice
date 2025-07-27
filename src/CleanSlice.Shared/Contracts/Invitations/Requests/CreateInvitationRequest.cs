using System.ComponentModel.DataAnnotations;

namespace CleanSlice.Shared.Contracts.Invitations.Requests;

public sealed record CreateInvitationRequest(
    [Required] 
    [EmailAddress] 
    string Email,
    
    [Required] 
    string RoleName,
    
    int? ExpirationDays = 7
    );
