using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.User.ValueObjects;

public sealed class UserId : AggregateRootId<Guid>
{
    private UserId()
    {
    }

    private UserId(Guid value)
    {
        Value = value;
    }

    public override Guid Value { get; protected set; }

    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Create(Guid value)
    {
        return new UserId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}