using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public string Type { get; private set; } = string.Empty;
    
    public string Content { get; private set; } = string.Empty;
    
    public DateTimeOffset OccurredOn { get; private set; }
    
    public DateTimeOffset? ProcessedOn { get; private set; }
    
    public string? Error { get; private set; }

    private OutboxMessage() { }

    private OutboxMessage(Guid id, string type, string content, DateTimeOffset occurredOn)
    {
        Id = id;
        Type = type;
        Content = content;
        OccurredOn = occurredOn;
    }
    
    public static OutboxMessage Create(Guid id, string type, string content)
    {
        if (id == Guid.Empty)
            throw new ValidationException(nameof(id), "OutboxMessage ID cannot be empty");

        if (string.IsNullOrWhiteSpace(type))
            throw new ValidationException(nameof(type), "OutboxMessage type cannot be empty");

        if (string.IsNullOrWhiteSpace(content))
            throw new ValidationException(nameof(content), "OutboxMessage content cannot be empty");

        return new OutboxMessage(id, type.Trim(), content.Trim(), DateTimeOffset.UtcNow);
    }

    public void MarkAsProcessed()
    {
        if (ProcessedOn.HasValue)
            return; // Already processed

        ProcessedOn = DateTimeOffset.UtcNow;
        Error = null;
    }

    public void MarkAsFailed(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ValidationException(nameof(error), "Error message cannot be empty");

        Error = error.Trim();
        ProcessedOn = null; // Reset processed time on failure
    }
}
