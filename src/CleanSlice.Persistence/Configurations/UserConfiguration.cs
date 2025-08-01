using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class UserConfiguration : AuditableEntityWithSoftDeleteConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("users");

        // ExternalIdentityId value object
        builder.OwnsOne(u => u.ExternalIdentityId, externalIdBuilder =>
        {
            externalIdBuilder.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("external_identity_id");

            externalIdBuilder.Property(e => e.Provider)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName("identity_provider");
        });

        builder.OwnsOne(u => u.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("email");
        });

        // FullName value object
        builder.OwnsOne(u => u.FullName, fullNameBuilder =>
        {
            fullNameBuilder.Property(fn => fn.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("first_name");

            fullNameBuilder.Property(fn => fn.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("last_name");
        });

        builder.Property(u => u.LastLogin);

        // Indexes
        builder.HasIndex(u => new { u.ExternalIdentityId.Value, u.ExternalIdentityId.Provider })
            .IsUnique()
            .HasDatabaseName("IX_Users_ExternalIdentityId_Provider");

        builder.HasIndex(u => u.Email.Value)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        // Relationships
        builder.HasMany(u => u.UserTenants)
            .WithOne(ut => ut.User)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
