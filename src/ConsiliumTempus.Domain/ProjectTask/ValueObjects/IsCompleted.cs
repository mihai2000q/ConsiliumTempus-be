using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Workspace.ValueObjects;

public sealed class IsCompleted : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private IsCompleted()
    {
    }

    private IsCompleted(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static IsCompleted Create(bool value)
    {
        return new IsCompleted(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}