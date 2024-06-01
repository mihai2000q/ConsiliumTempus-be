namespace ConsiliumTempus.Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();
    
    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;

        var valueObject = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(valueObject.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null) return true;
        return left is not null && left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null) return true;
        return left is not null && left.Equals(right);
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