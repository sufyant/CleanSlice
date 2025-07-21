namespace CleanSlice.Domain.Common.ValueObjects;

public sealed class FullName
{
    public string FirstName { get; }
    public string LastName { get; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

        return new FullName(
            firstName.Trim(),
            lastName.Trim());
    }

    public string GetInitials() => $"{FirstName[0]}{LastName[0]}".ToUpperInvariant();

    public override string ToString() => $"{FirstName} {LastName}";
}
