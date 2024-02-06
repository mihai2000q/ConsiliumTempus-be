using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.User.ValueObjects;

public class Name : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private Name()
    {
    }

    private Name(string first, string last)
    {
        First = first;
        Last = last;
    }

    public string First { get; } = string.Empty;
    public string Last { get; } = string.Empty;

    public static Name Create(string first, string last)
    {
        return new Name(first, last);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return First;
        yield return Last;
    }
}