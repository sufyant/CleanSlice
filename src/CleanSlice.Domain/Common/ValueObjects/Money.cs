using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Common.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ValidationException(nameof(amount), "Amount cannot be negative");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ValidationException(nameof(currency), "Currency cannot be null or empty");

        if (currency.Length != 3)
            throw new ValidationException(nameof(currency), "Currency must be 3 characters long (ISO 4217)");

        return new Money(amount, currency.ToUpperInvariant());
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new BusinessRuleViolationException("Cannot add money with different currencies");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new BusinessRuleViolationException("Cannot subtract money with different currencies");

        if (Amount < other.Amount)
            throw new BusinessRuleViolationException("Insufficient funds");

        return new Money(Amount - other.Amount, Currency);
    }

    public override string ToString() => $"{Amount:F2} {Currency}";

    public override bool Equals(object? obj)
    {
        return obj is Money other &&
               Amount == other.Amount &&
               Currency == other.Currency;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public static bool operator ==(Money? left, Money? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Money? left, Money? right)
    {
        return !Equals(left, right);
    }

    public static Money operator +(Money left, Money right)
    {
        return left.Add(right);
    }

    public static Money operator -(Money left, Money right)
    {
        return left.Subtract(right);
    }
}
