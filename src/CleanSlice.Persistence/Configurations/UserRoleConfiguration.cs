using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base.Base;
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
            .IsRequired();

        builder.Property(ur => ur.RoleId)
            .IsRequired();

        builder.Property(ur => ur.AssignedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique();

        // Relationships are configured in User and Role configurations
    }
}
