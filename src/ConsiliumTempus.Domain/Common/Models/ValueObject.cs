namespace ConsiliumTempus.Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;

        var valueObject = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(valueObject.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public override string ToString()
    {
        return GetEqualityComponents()
            .Aggregate("", (current, c) => current + c + ", ")
            .TrimEnd(' ', ',');
    }
}