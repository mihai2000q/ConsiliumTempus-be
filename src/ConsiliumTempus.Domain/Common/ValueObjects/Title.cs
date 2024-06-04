using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class Title : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Title()
    {
    }

    private Title(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static Title Create(string value)
    {
        return new Title(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}