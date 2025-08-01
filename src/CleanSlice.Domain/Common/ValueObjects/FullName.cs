using CleanSlice.Domain.Common.Exceptions;

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
            throw new ValidationException(nameof(firstName), "First name cannot be null or empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException(nameof(lastName), "Last name cannot be null or empty");

        return new FullName(
            firstName.Trim(),
            lastName.Trim());
    }

    public string GetInitials() => $"{FirstName[0]}{LastName[0]}".ToUpperInvariant();

    public override string ToString() => $"{FirstName} {LastName}";

    public override bool Equals(object? obj)
    {
        return obj is FullName other && 
               FirstName == other.FirstName && 
               LastName == other.LastName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName);
    }

    public static bool operator ==(FullName? left, FullName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FullName? left, FullName? right)
    {
        return !Equals(left, right);
    }
}
