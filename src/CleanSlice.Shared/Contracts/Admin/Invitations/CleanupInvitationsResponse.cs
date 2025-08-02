namespace CleanSlice.Shared.Contracts.Admin.Invitations;

public sealed record CleanupInvitationsResponse
{
    public int CleanedUpCount { get; init; }
    public DateTime CleanupDate { get; init; }
}
