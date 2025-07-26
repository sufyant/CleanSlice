using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class PermissionConfiguration : AuditableEntityConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);

        builder.ToTable("permissions");

        builder.OwnsOne(p => p.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(p => p.Name.Value)
            .IsUnique();

        builder.HasIndex(p => p.Category);

        // Relationships
        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
