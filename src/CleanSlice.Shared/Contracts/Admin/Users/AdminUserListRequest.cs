namespace CleanSlice.Shared.Contracts.Admin.Users;

public sealed record AdminUserListRequest
{
    public string? SearchTerm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; } = "Email";
    public bool SortDescending { get; init; } = false;
    public bool? IsActive { get; init; } // Filter by active status
    public bool? IsSuperAdmin { get; init; } // Filter by super admin status
    public Guid? TenantId { get; init; } // Filter by tenant membership
}
