using CleanSlice.Domain.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.TenantManagement.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Domain)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.ConnectionString)
            .IsRequired()
            .HasMaxLength(500);

        // Unique constraints
        builder.HasIndex(t => t.Domain).IsUnique();
        builder.HasIndex(t => t.Slug).IsUnique();

        // Table name
        builder.ToTable("tenants");
    }
}
