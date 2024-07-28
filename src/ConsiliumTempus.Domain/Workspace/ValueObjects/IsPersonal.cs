using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Workspace.ValueObjects;

public sealed class IsPersonal : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private IsPersonal()
    {
    }

    private IsPersonal(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static IsPersonal Create(bool value)
    {
        return new IsPersonal(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}