using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.UserAggregate.ValueObjects;

public class Name : ValueObject
{
    private Name()
    {
    }

    private Name(string first, string last)
    {
        First = first;
        Last = last;
    }

    public string First { get; private set; } = string.Empty;
    public string Last { get; private set; } = string.Empty;

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