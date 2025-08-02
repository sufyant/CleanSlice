using CleanSlice.Domain.Outbox;
using CleanSlice.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanSlice.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : BaseEntityConfiguration<OutboxMessage>
{
    public override void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        base.Configure(builder);

        builder.ToTable("outbox_messages");

        builder.Property(o => o.Type)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("type");

        builder.Property(o => o.Content)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName("content");

        builder.Property(o => o.OccurredOn)
            .IsRequired()
            .HasColumnName("occurred_on");

        builder.Property(o => o.ProcessedOn)
            .HasColumnName("processed_on");

        builder.Property(o => o.Error)
            .HasMaxLength(1000)
            .HasColumnName("error");

        // Indexes
        builder.HasIndex(o => o.OccurredOn)
            .HasDatabaseName("IX_OutboxMessages_OccurredOn");

        builder.HasIndex(o => o.ProcessedOn)
            .HasDatabaseName("IX_OutboxMessages_ProcessedOn");

        builder.HasIndex(o => new { o.ProcessedOn, o.OccurredOn })
            .HasDatabaseName("IX_OutboxMessages_ProcessedOn_OccurredOn");
    }
} 