namespace CleanSlice.Shared.Contracts.Admin.Tenants;

public sealed record TenantDetailResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Domain { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }
    public TenantStatistics Statistics { get; init; } = null!;
}

public sealed record TenantStatistics
{
    public int TotalUsers { get; init; }
    public int ActiveUsers { get; init; }
    public int TotalRoles { get; init; }
    public int TotalInvitations { get; init; }
    public int PendingInvitations { get; init; }
    public DateTime? LastUserActivity { get; init; }
}
