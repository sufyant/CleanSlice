using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class InvitationConfiguration : AuditableTenantEntityConfiguration<Invitation>
{
    public override void Configure(EntityTypeBuilder<Invitation> builder)
    {
        base.Configure(builder);

        builder.ToTable("invitations");

        // Email value object
        builder.OwnsOne(i => i.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("email");
        });

        builder.Property(i => i.RoleId)
            .IsRequired()
            .HasColumnName("role_id");

        builder.Property(i => i.InvitedBy)
            .IsRequired()
            .HasColumnName("invited_by");

        builder.Property(i => i.Token)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("token");

        builder.Property(i => i.ExpiresAt)
            .IsRequired()
            .HasColumnName("expires_at");

        builder.Property(i => i.IsUsed)
            .IsRequired()
            .HasColumnName("is_used");

        builder.Property(i => i.UsedAt)
            .HasColumnName("used_at");

        builder.Property(i => i.UsedBy)
            .HasColumnName("used_by");

        // Indexes
        builder.HasIndex(i => i.Token)
            .IsUnique()
            .HasDatabaseName("IX_Invitations_Token");

        builder.HasIndex(i => new { i.TenantId, i.Email.Value })
            .HasDatabaseName("IX_Invitations_TenantId_Email");

        builder.HasIndex(i => i.ExpiresAt)
            .HasDatabaseName("IX_Invitations_ExpiresAt");

        // Relationships
        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(i => i.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
