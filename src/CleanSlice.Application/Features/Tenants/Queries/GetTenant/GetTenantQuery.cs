using CleanSlice.Application.Abstractions.Caching;
using CleanSlice.Application.Features.Tenants.DTOs;

namespace CleanSlice.Application.Features.Tenants.Queries.GetTenant;

public sealed record GetTenantQuery(Guid TenantId) : ICachedQuery<TenantDto>
{
    public string CacheKey => $"tenant-{TenantId}";
    
    public TimeSpan? Expiration => TimeSpan.FromMinutes(30);
}
