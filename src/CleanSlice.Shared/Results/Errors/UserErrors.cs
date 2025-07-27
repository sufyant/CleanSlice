namespace CleanSlice.Shared.Results.Errors;

public static class UserErrors
{
    public static Error EmailAlreadyExists => Error.Conflict(
        "User.EmailAlreadyExists", 
        "A user with this email already exists");
    
    public static Error FailedToCreateInAzure => Error.Failure(
        "User.FailedToCreateInAzure",
        "Failed to create user in Azure Entra ID");

    public static Error FailedToGetAzureUserId => Error.Failure(
        "User.FailedToGetAzureUserId",
        "Failed to get Azure Entra ID user ID");

    public static Error SyncFailed => Error.Failure(
        "User.SyncFailed",
        "Failed to sync user from Azure token");

    public static Error RegistrationFailed => Error.Failure(
        "User.RegistrationFailed",
        "Failed to register user");

    public static Error AlreadyExists => Error.Conflict(
        "User.AlreadyExists",
        "User with this email already exists");

    public static Error NotFound => Error.NotFound(
        "User.NotFound",
        "User not found");
}
