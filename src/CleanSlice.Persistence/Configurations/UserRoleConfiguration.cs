using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder);

        builder.ToTable("user_roles");

        builder.Property(ur => ur.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(ur => ur.RoleId)
            .IsRequired()
            .HasColumnName("role_id");

        builder.Property(ur => ur.TenantId)
            .IsRequired()
            .HasColumnName("tenant_id");

        builder.Property(ur => ur.AssignedAt)
            .IsRequired()
            .HasColumnName("assigned_at");

        // Indexes
        builder.HasIndex(ur => new { ur.UserId, ur.RoleId, ur.TenantId })
            .IsUnique()
            .HasDatabaseName("IX_UserRoles_UserId_RoleId_TenantId");

        builder.HasIndex(ur => new { ur.UserId, ur.TenantId })
            .HasDatabaseName("IX_UserRoles_UserId_TenantId");

        // Relationships
        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
