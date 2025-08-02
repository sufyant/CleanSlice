namespace CleanSlice.Shared.Contracts.Admin.Tenants;

public sealed record TenantListResponse
{
    public TenantSummary[] Tenants { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

public sealed record TenantSummary
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Domain { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int UserCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }
}
