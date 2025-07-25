using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Tenants.Events;

public sealed record TenantUpdatedDomainEvent(Guid TenantId) : IDomainEvent;
