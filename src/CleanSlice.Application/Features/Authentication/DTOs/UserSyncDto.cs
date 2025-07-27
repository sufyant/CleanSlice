namespace CleanSlice.Application.Features.Authentication.DTOs;

public sealed record UserSyncDto(
    Guid Id,
    string IdentityId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    bool IsActive,
    bool IsNewlyCreated);
