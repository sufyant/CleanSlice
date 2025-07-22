namespace CleanSlice.Shared.Results.Errors;

public static class TenantErrors
{
    // Repository/Query Results
    public static Error NotFound => Error.NotFound(
        "Tenant.NotFound",
        "Tenant was not found");

    // Cross-Aggregate Business Rules
    public static Error AlreadyExists => Error.Conflict(
        "Tenant.AlreadyExists", 
        "Tenant with this name already exists");

    public static Error NameAlreadyTaken => Error.Conflict(
        "Tenant.NameAlreadyTaken",
        "Another tenant with this name already exists");

    // Infrastructure/Technical Errors
    public static Error DatabaseConnectionFailed => Error.Failure(
        "Tenant.DatabaseConnectionFailed",
        "Could not connect to tenant database");

    public static Error InvalidConnectionString => Error.Problem(
        "Tenant.InvalidConnectionString", 
        "The provided connection string format is invalid");

    // Authorization/Business Context
    public static Error CannotDeleteSystemTenant => Error.Problem(
        "Tenant.CannotDeleteSystem",
        "System tenant cannot be deleted");

    public static Error MaxTenantLimitReached => Error.Conflict(
        "Tenant.MaxLimitReached",
        "Maximum number of tenants reached for this subscription");
}
