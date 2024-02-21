using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class IsPrivate : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private IsPrivate()
    {
    }

    private IsPrivate(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static IsPrivate Create(bool value)
    {
        return new IsPrivate(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}