using CleanSlice.Domain.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.TenantManagement.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);

        // Value objects configuration
        builder.OwnsOne(t => t.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        builder.OwnsOne(t => t.Domain, domainBuilder =>
        {
            domainBuilder.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("domain");
        });

        builder.OwnsOne(t => t.Slug, slugBuilder =>
        {
            slugBuilder.Property(s => s.Value)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("slug");
        });

        builder.Property(t => t.ConnectionString)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("connection_string");

        // Unique constraints
        builder.HasIndex(t => t.Domain.Value).IsUnique();
        builder.HasIndex(t => t.Slug.Value).IsUnique();

        // Table name
        builder.ToTable("tenants");
    }
}
