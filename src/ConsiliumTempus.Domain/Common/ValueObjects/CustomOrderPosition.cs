using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class CustomOrderPosition : ValueObject
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

    public static CustomOrderPosition operator +(CustomOrderPosition a, int b)
    {
        return Create(a.Value + b);
    }

    public static CustomOrderPosition operator -(CustomOrderPosition a, int b)
    {
        return Create(a.Value - b);
    }
}