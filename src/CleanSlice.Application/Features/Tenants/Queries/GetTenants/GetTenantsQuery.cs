using CleanSlice.Application.Abstractions.Caching;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Tenants.DTOs;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Features.Tenants.Queries.GetTenants;

public sealed record GetTenantsQuery(int Page, int PageSize, string? SearchTerm) : ICachedQuery<PagedResult<TenantDto>>
{
    public string CacheKey => $"tenants:page:{Page}:size:{PageSize}:search:{SearchTerm}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
