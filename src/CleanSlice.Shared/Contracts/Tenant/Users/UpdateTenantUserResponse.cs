namespace CleanSlice.Shared.Contracts.Tenant.Users;

public sealed record UpdateTenantUserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public DateTime LastModifiedAt { get; init; }
}
