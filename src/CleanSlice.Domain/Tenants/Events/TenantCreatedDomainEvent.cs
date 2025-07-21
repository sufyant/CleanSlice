using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Domain.Tenants.Events;

public record TenantCreatedDomainEvent(Guid TenantId, string Name) : IDomainEvent;
