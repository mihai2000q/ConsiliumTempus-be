using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Authentication.ValueObjects;

public sealed class RefreshTokenId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private RefreshTokenId()
    {
    }

    private RefreshTokenId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static RefreshTokenId CreateUnique()
    {
        return new RefreshTokenId(Guid.NewGuid());
    }

    public static RefreshTokenId Create(Guid value)
    {
        return new RefreshTokenId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}