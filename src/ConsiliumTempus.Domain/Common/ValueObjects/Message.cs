using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class Message : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private Message()
    {
    }

    private Message(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static Message Create(string value)
    {
        return new Message(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}