namespace CleanSlice.Shared.Entities;

public sealed class OutboxMessage : BaseEntity
{
    public string Type { get; private set; } = string.Empty;
    
    public string Content { get; private set; } = string.Empty;
    
    public DateTimeOffset OccurredOn { get; private set; }
    
    public DateTimeOffset? ProcessedOn { get; private set; }
    
    public string? Error { get; private set; }

    private OutboxMessage() { }

    public OutboxMessage(
        Guid id,
        string type, 
        string content, 
        DateTimeOffset occurredOn)
    {
        Id = id;
        Type = type;
        Content = content;
        OccurredOn = occurredOn;
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
