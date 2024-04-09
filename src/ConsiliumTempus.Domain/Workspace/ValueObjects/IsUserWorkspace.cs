using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Workspace.ValueObjects;

public class IsUserWorkspace : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private IsUserWorkspace()
    {
    }

    private IsUserWorkspace(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static IsUserWorkspace Create(bool value)
    {
        return new IsUserWorkspace(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}