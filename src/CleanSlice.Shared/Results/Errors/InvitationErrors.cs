namespace CleanSlice.Shared.Results.Errors;

public static class InvitationErrors
{
    public static Error NotFound => Error.NotFound(
        "Invitation.NotFound", 
        "Invitation not found");
    
    public static Error InvalidToken => Error.Failure(
        "Invitation.InvalidToken", 
        "Invitation token is required");
    
    public static Error Expired => Error.Failure(
        "Invitation.Expired", 
        "Invitation has expired");
    
    public static Error AlreadyUsed => Error.Failure(
        "Invitation.AlreadyUsed", 
        "Invitation has already been used");
    
    public static Error EmailMismatch => Error.Failure(
        "Invitation.EmailMismatch", 
        "Email does not match invitation");
    
    public static Error AlreadyExists => Error.Conflict(
        "Invitation.AlreadyExists", 
        "Pending invitation already exists for this email");
    
    public static Error CreateFailed => Error.Failure(
        "Invitation.CreateFailed", 
        "Failed to create invitation");
    
    public static Error ResolveFailed => Error.Failure(
        "Invitation.ResolveFailed", 
        "Failed to resolve invitation");
    
    public static Error CannotCancelUsed => Error.Failure(
        "Invitation.CannotCancelUsed", 
        "Cannot cancel an invitation that has already been used");
    
    public static Error CancelFailed => Error.Failure(
        "Invitation.CancelFailed", 
        "Failed to cancel invitation");
    
    public static Error GetFailed => Error.Failure(
        "Invitation.GetFailed", 
        "Failed to get invitation");
    
    public static Error GetAllFailed => Error.Failure(
        "Invitation.GetAllFailed", 
        "Failed to get invitations");
}
