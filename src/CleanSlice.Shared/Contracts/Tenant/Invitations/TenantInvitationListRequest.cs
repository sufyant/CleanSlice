namespace CleanSlice.Shared.Contracts.Tenant.Invitations;

public sealed record TenantInvitationListRequest
{
    public string? SearchTerm { get; init; } // Email search
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; } = "CreatedAt";
    public bool SortDescending { get; init; } = true;
    public bool? IsUsed { get; init; } // Filter by usage status
    public bool? IsExpired { get; init; } // Filter by expiration
    public Guid? RoleId { get; init; } // Filter by role
}
