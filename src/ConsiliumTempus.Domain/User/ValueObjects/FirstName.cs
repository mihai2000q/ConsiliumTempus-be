using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.User.ValueObjects;

public sealed class FirstName : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private FirstName()
    {
    }

    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static FirstName Create(string value)
    {
        return new FirstName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}