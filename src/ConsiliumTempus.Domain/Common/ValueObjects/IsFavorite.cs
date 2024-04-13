using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

public sealed class IsFavorite : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private IsFavorite()
    {
    }

    private IsFavorite(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static IsFavorite Create(bool value)
    {
        return new IsFavorite(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}