namespace CleanSlice.Shared.Contracts.Admin.Invitations;

public sealed record AdminInvitationListRequest
{
    public string? SearchTerm { get; init; } // Email search
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; } = "CreatedAt";
    public bool SortDescending { get; init; } = true;
    public Guid? TenantId { get; init; } // Filter by tenant
    public bool? IsUsed { get; init; } // Filter by usage status
    public bool? IsExpired { get; init; } // Filter by expiration
}
