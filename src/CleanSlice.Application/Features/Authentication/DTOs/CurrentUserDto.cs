namespace CleanSlice.Application.Features.Authentication.DTOs;

public sealed record CurrentUserDto(
    Guid Id,
    string IdentityId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    bool IsActive,
    List<string> Roles,
    List<string> Permissions);
