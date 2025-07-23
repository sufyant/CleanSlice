using CleanSlice.Shared.Results;

namespace CleanSlice.Domain.Tenants;

public static class TenantErrors
{
    public static readonly Error TenantAlreadyExists = new Error(
        "TenantAlreadyExists",
        "A tenant with the same name or domain already exists.",
        ErrorType.Failure
    );
}
