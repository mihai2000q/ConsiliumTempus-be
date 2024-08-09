using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Models;

public abstract class AggregateRootId<TId> : ValueObject, IAggregateRootId
    where TId : notnull
{
    public abstract TId Value { get; protected set; }

    public override string ToString()
    {
        return Value.ToString() ?? "No Id";
    }
}