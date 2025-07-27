namespace CleanSlice.Application.Features.Registration.DTOs;

public sealed record RegistrationDto(
    Guid UserId,
    string IdentityId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    string RoleName);
