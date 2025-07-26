using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class UserConfiguration : AuditableTenantEntityWithSoftDeleteConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("users");

        builder.Property(u => u.IdentityId)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(u => u.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("email");
        });

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.IsActive)
            .IsRequired();

        // Indexes
        builder.HasIndex(u => u.IdentityId)
            .IsUnique();

        builder.HasIndex(u => new { u.TenantId, u.Email.Value })
            .IsUnique();

        // Relationships
        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
