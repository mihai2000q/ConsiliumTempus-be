using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.UserAggregate.ValueObjects;

public class UserId : AggregateRootId<Guid>
{
    public sealed override Guid Value { get; protected set; }

    private UserId()
    {
    }

    private UserId(Guid value)
    {
        Value = value;
    }

    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}