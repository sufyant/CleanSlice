using CleanSlice.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations.Base;

public abstract class TenantBaseEntityConfiguration<T> : BaseEntityConfiguration<T> 
    where T : TenantBaseEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        
        builder.Property(e => e.TenantId).IsRequired();
    }
}
