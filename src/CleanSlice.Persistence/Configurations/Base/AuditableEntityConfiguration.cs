using CleanSlice.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations.Base;

public abstract class AuditableEntityConfiguration<T> : BaseEntityConfiguration<T> 
    where T : AuditableEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        
        builder.Property(e => e.CreatedBy).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.LastModifiedBy);
        builder.Property(e => e.LastModifiedAt);
    }
}
