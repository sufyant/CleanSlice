using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class RoleConfiguration : AuditableTenantEntityWithSoftDeleteConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.ToTable("roles");

        builder.OwnsOne(r => r.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.IsSystemRole)
            .IsRequired();

        // Indexes
        builder.HasIndex(r => new { r.TenantId, r.Name.Value })
            .IsUnique();

        // Relationships
        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
