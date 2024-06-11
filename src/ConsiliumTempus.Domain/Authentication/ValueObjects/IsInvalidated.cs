using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Authentication.ValueObjects;

public sealed class IsInvalidated : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private IsInvalidated()
    {
    }

    private IsInvalidated(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static IsInvalidated Create(bool value)
    {
        return new IsInvalidated(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}