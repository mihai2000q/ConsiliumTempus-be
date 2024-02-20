using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class Order : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private Order()
    {
    }

    private Order(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Order Create(int value)
    {
        return new Order(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}