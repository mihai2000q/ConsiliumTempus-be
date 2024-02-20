using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.User.ValueObjects;

public sealed class LastName : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private LastName()
    {
    }

    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static LastName Create(string value)
    {
        return new LastName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}