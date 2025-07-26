namespace CleanSlice.Shared.Results.Errors;

public static class RoleErrors
{
    public static Error NameAlreadyExists => Error.Conflict(
        "Role.NameAlreadyExists", 
        "A role with this name already exists");
    
    public static Error FailedToCreateInKeycloak => Error.Failure(
        "Role.FailedToCreateInKeycloak", 
        "Failed to create role in Keycloak");
}
