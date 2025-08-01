using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base.Base;
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
            .IsRequired();

        builder.Property(ut => ut.TenantId)
            .IsRequired();

        builder.Property(ut => ut.IsPrimary)
            .IsRequired();

        builder.Property(ut => ut.JoinedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(ut => new { ut.UserId, ut.TenantId })
            .IsUnique();

        builder.HasIndex(ut => new { ut.UserId, ut.IsPrimary })
            .HasFilter("is_primary = true"); // Only one primary tenant per user

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
