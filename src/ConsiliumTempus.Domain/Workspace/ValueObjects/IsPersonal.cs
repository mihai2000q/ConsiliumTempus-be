using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Workspace.ValueObjects;

public class IsPersonal : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
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