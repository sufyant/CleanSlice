using CleanSlice.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyTenantFilter(this ModelBuilder modelBuilder, Guid tenantId)
    {
        // Apply tenant filter to all tenant-aware base entities
        // This automatically covers ALL entities that inherit from these bases
        // No need to manually add each entity - inheritance handles it!

        // TenantBaseEntity - covers all entities with TenantId (including UserRole now)
        modelBuilder.Entity<TenantBaseEntity>().HasQueryFilter(e => e.TenantId == tenantId);

        // AuditableTenantEntity - covers auditable tenant entities
        modelBuilder.Entity<AuditableTenantEntity>().HasQueryFilter(e => e.TenantId == tenantId);

        // AuditableTenantEntityWithSoftDelete - covers auditable tenant entities with soft delete
        modelBuilder.Entity<AuditableTenantEntityWithSoftDelete>().HasQueryFilter(e => e.TenantId == tenantId);
    }
    
    public static void ApplySoftDeleteFilter(this ModelBuilder modelBuilder)
    {
        // Apply a global filter for soft-deleted entities
        modelBuilder.Entity<AuditableEntityWithSoftDelete>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<AuditableTenantEntityWithSoftDelete>().HasQueryFilter(e => e.DeletedAt == null);
    }
}
