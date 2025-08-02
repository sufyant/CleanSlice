namespace CleanSlice.Shared.Contracts.Tenant.Invitations;

public sealed record TenantInvitationListResponse
{
    public TenantInvitationSummary[] Invitations { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

public sealed record TenantInvitationSummary
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string InvitedByEmail { get; init; } = string.Empty;
    public bool IsUsed { get; init; }
    public bool IsExpired { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ExpiresAt { get; init; }
    public DateTime? UsedAt { get; init; }
}
