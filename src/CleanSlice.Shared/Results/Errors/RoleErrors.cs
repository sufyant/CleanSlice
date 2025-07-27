namespace CleanSlice.Shared.Results.Errors;

public static class RoleErrors
{
    public static Error NameAlreadyExists => Error.Conflict(
        "Role.NameAlreadyExists", 
        "A role with this name already exists");
    
    public static Error FailedToCreateInAzure => Error.Failure(
        "Role.FailedToCreateInAzure",
        "Failed to create role in Azure Entra ID");

    public static Error NotFound => Error.NotFound(
        "Role.NotFound",
        "Role not found");
}
