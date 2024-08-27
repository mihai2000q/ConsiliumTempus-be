using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class CustomOrderPosition : ValueObject, IComparable<CustomOrderPosition>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private CustomOrderPosition()
    {
    }

    private CustomOrderPosition(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static CustomOrderPosition Create(int value)
    {
        return new CustomOrderPosition(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public int CompareTo(CustomOrderPosition? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        return other is null 
            ? 1
            : Value.CompareTo(other.Value);
    }

    public static CustomOrderPosition operator ++(CustomOrderPosition a) => Create(a.Value + 1);
    public static CustomOrderPosition operator --(CustomOrderPosition a) => Create(a.Value - 1);
    public static CustomOrderPosition operator +(CustomOrderPosition a, int b) => Create(a.Value + b);
    public static CustomOrderPosition operator -(CustomOrderPosition a, int b) => Create(a.Value - b);
    public static bool operator <(CustomOrderPosition a, CustomOrderPosition b) => a.Value < b.Value;
    public static bool operator <=(CustomOrderPosition a, CustomOrderPosition b) => a.Value <= b.Value;
    public static bool operator >(CustomOrderPosition a, CustomOrderPosition b) => a.Value > b.Value;
    public static bool operator >=(CustomOrderPosition a, CustomOrderPosition b) => a.Value >= b.Value;
}