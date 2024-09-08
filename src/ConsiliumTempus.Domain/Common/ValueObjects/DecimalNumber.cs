using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class DecimalNumber : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private DecimalNumber()
    {
    }

    private DecimalNumber(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static DecimalNumber Create(decimal value)
    {
        return new DecimalNumber(value);
    }

    public DecimalNumber Copy()
    {
        return new DecimalNumber(Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}