namespace CleanSlice.Shared.Contracts.Tenant.Users;

public sealed record TenantUserListRequest
{
    public string? SearchTerm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; } = "Email";
    public bool SortDescending { get; init; } = false;
    public bool? IsActive { get; init; } // Filter by active status
    public Guid? RoleId { get; init; } // Filter by role
}
