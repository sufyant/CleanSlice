using CleanSlice.Application.Abstractions.Caching;
using CleanSlice.Application.Features.Users.DTOs;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery(int Page, int PageSize, string? SearchTerm) : ICachedQuery<PagedResult<UserDto>>
{
    public string CacheKey => $"users:page:{Page}:size:{PageSize}:search:{SearchTerm}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}
