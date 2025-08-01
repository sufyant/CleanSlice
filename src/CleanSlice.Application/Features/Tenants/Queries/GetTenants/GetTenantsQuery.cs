using CleanSlice.Application.Abstractions.Caching;
using CleanSlice.Application.Features.Tenants.DTOs;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Features.Tenants.Queries.GetTenants;

public sealed record GetTenantsQuery(PagedRequest Request) : ICachedQuery<PagedResult<TenantDto>>
{
    public string CacheKey => $"tenants:page:{Request.Page}:size:{Request.PageSize}:search:{Request.SearchTerm}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
