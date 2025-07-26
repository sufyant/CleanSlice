namespace CleanSlice.Shared.Results.Errors;

public static class UserErrors
{
    public static Error EmailAlreadyExists => Error.Conflict(
        "User.EmailAlreadyExists", 
        "A user with this email already exists");
    
    public static Error FailedToCreateInKeycloak => Error.Failure(
        "User.FailedToCreateInKeycloak", 
        "Failed to create user in Keycloak");
    
    public static Error FailedToGetKeycloakUserId => Error.Failure(
        "User.FailedToGetKeycloakUserId", 
        "Failed to get Keycloak user ID");
}
