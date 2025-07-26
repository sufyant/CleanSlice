using CleanSlice.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations.Base.Base;

public abstract class AuditableEntityWithSoftDeleteConfiguration<T> : BaseEntityConfiguration<T> 
    where T : AuditableEntityWithSoftDelete 
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        
        builder.Property(e => e.CreatedBy).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.LastModifiedBy);
        builder.Property(e => e.LastModifiedAt);
        builder.Property(e => e.DeletedAt);
        builder.Property(e => e.DeletedBy);
    }
}
