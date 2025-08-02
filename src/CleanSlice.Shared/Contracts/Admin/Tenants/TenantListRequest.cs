namespace CleanSlice.Shared.Contracts.Admin.Tenants;

public sealed record TenantListRequest
{
    public string? SearchTerm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; } = "Name";
    public bool SortDescending { get; init; } = false;
    public bool? IsActive { get; init; } // Filter by active status
}
