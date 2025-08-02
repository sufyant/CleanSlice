namespace CleanSlice.Shared.Contracts.Admin.Users;

public sealed record SuperAdminListResponse
{
    public SuperAdminInfo[] SuperAdmins { get; init; } = [];
    public int TotalCount { get; init; }
}

public sealed record SuperAdminInfo
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime? LastLogin { get; init; }
    public DateTime PromotedAt { get; init; }
}
