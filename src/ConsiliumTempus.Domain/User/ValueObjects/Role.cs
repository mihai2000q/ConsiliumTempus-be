using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.User.ValueObjects;

public sealed class Role : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Role()
    {
    }

    private Role(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static Role Create(string value)
    {
        return new Role(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}