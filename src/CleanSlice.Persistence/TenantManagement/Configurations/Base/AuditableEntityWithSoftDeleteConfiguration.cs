using CleanSlice.Shared.Entities;
using CleanSlice.Shared.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.TenantManagement.Configurations.Base;

public abstract class AuditableEntityWithSoftDeleteConfiguration<T> : AuditableEntityConfiguration<T> 
    where T : AuditableEntity, ISoftDelete 
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        
        builder.Property(e => e.DeletedAt);
        builder.Property(e => e.DeletedBy);
    }
}
