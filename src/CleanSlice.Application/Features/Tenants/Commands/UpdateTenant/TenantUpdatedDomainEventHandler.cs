using CleanSlice.Application.Abstractions.Caching;
using CleanSlice.Domain.Tenants.Events;
using MediatR;

namespace CleanSlice.Application.Features.Tenants.Commands.UpdateTenant;

internal sealed class TenantUpdatedDomainEventHandler(
    ICacheService cacheService
    ) : INotificationHandler<TenantUpdatedDomainEvent>
{
    public async Task Handle(TenantUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Remove all tenant-related caches
        await cacheService.RemoveByPatternAsync("tenants:*", cancellationToken);

        // Also remove specific tenant cache if needed
        await cacheService.RemoveAsync($"tenant:{notification.TenantId}", cancellationToken);
    }
}
