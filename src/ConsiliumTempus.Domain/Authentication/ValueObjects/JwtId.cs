using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Authentication.ValueObjects;

public sealed class JwtId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private JwtId()
    {
    }

    private JwtId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static JwtId Create(Guid value)
    {
        return new JwtId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}