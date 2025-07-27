using CleanSlice.Application.Abstractions.Caching;
using CleanSlice.Application.Features.Invitations.DTOs;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Features.Invitations.Queries.GetInvitations;

public sealed record GetInvitationsQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null) : ICachedQuery<PagedResult<InvitationDto>>
{
    public string CacheKey => $"invitations:page:{Page}:size:{PageSize}:search:{SearchTerm}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}
