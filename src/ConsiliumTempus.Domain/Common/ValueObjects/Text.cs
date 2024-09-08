using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class Text : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Text()
    {
    }

    private Text(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static Text Create(string value)
    {
        return new Text(value);
    }

    public Text Copy()
    {
        return new Text(Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}