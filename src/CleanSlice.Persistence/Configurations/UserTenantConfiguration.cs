using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class UserTenantConfiguration : AuditableEntityConfiguration<UserTenant>
{
    public override void Configure(EntityTypeBuilder<UserTenant> builder)
    {
        base.Configure(builder);

        builder.ToTable("user_tenants");

        builder.Property(ut => ut.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(ut => ut.TenantId)
            .IsRequired()
            .HasColumnName("tenant_id");

        builder.Property(ut => ut.IsPrimary)
            .IsRequired()
            .HasColumnName("is_primary");

        builder.Property(ut => ut.JoinedAt)
            .IsRequired()
            .HasColumnName("joined_at");

        // Indexes
        builder.HasIndex(ut => new { ut.UserId, ut.TenantId })
            .IsUnique()
            .HasDatabaseName("IX_UserTenants_UserId_TenantId");

        builder.HasIndex(ut => new { ut.UserId, ut.IsPrimary })
            .HasDatabaseName("IX_UserTenants_UserId_IsPrimary");

        // Relationships
        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserTenants)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ut => ut.Tenant)
            .WithMany()
            .HasForeignKey(ut => ut.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
