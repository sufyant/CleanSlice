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
        return new OutboxMessage(id, type, content, DateTimeOffset.UtcNow);
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTimeOffset.UtcNow;
        Error = null;
    }

    public void MarkAsFailed(string error)
    {
        Error = error;
        ProcessedOn = null;
    }
}
