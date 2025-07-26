namespace CleanSlice.Shared.Contracts.Users.Responses;

public sealed record UserResponse(
    Guid Id,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    bool IsActive,
    IEnumerable<string> Roles
    );
