namespace CleanSlice.Shared.Interfaces;

public interface ISoftDelete
{
    DateTimeOffset? DeletedAt { get; }
    Guid? DeletedBy { get; }
}
