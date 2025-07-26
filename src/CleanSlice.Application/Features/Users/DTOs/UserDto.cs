namespace CleanSlice.Application.Features.Users.DTOs;

public sealed record UserDto(
    Guid Id,
    string IdentityId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    bool IsActive,
    IEnumerable<string> Roles
    );
