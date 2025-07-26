namespace CleanSlice.Shared.Contracts.Roles.Responses;

public sealed record RoleResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    string Description,
    bool IsSystemRole,
    IEnumerable<string> Permissions
    );
