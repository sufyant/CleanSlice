namespace CleanSlice.Shared.Interfaces;

public interface ISoftDelete
{
    DateTimeOffset? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }
}
