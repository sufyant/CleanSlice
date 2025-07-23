using CleanSlice.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyTenantFilter(this ModelBuilder modelBuilder, Guid tenantId)
    {
        // Apply a global filter for tenant-based entities
        modelBuilder.Entity<TenantBaseEntity>().HasQueryFilter(e => e.TenantId == tenantId);
    }
    
    public static void ApplySoftDeleteFilter(this ModelBuilder modelBuilder)
    {
        // Apply a global filter for soft-deleted entities
        modelBuilder.Entity<AuditableEntityWithSoftDelete>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<AuditableTenantEntityWithSoftDelete>().HasQueryFilter(e => e.DeletedAt == null);
    }
}
