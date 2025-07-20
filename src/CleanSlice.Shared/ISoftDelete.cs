namespace CleanSlice.Shared;

public interface ISoftDelete
{
    DateTimeOffset? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }
}
