namespace CleanSlice.Application.Features.Roles.DTOs;

public sealed record RoleDto(
    Guid Id,
    Guid TenantId,
    string Name,
    string Description,
    bool IsSystemRole,
    IEnumerable<string> Permissions
    );
