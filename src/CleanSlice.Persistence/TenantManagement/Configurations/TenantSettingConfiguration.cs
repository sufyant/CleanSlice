using CleanSlice.Domain.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.TenantManagement.Configurations;

public class TenantSettingConfiguration : IEntityTypeConfiguration<TenantSetting>
{
    public void Configure(EntityTypeBuilder<TenantSetting> builder)
    {
        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ts => ts.Value)
            .IsRequired()
            .HasMaxLength(1000);

        // Composite unique constraint: TenantId + Key
        builder.HasIndex(ts => new { ts.TenantId, ts.Key }).IsUnique();

        // Foreign key relationship
        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(ts => ts.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Table name
        builder.ToTable("tenant_settings");
    }
}
