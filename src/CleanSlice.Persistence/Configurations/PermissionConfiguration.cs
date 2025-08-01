using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class PermissionConfiguration : AuditableEntityConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);

        builder.ToTable("permissions");

        // PermissionName value object
        builder.OwnsOne(p => p.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("description");

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("category");

        // Indexes
        builder.HasIndex(p => p.Name.Value)
            .IsUnique()
            .HasDatabaseName("IX_Permissions_Name");

        builder.HasIndex(p => p.Category)
            .HasDatabaseName("IX_Permissions_Category");

        // Relationships
        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
