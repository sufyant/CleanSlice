using CleanSlice.Shared.Entities;
using CleanSlice.Shared.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.TenantManagement.Configurations.Base;

public abstract class AuditableTenantEntityWithSoftDeleteConfiguration<T> : AuditableEntityWithSoftDeleteConfiguration<T> 
    where T : AuditableEntity, IMustHaveTenant, ISoftDelete
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        
        builder.Property(e => e.TenantId).IsRequired();
    }
}
