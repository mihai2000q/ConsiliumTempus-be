namespace ConsiliumTempus.Domain.Common.Models;

public class AggregateRoot<TId, TIdType> : Entity<TId>
    where TId : AggregateRootId<TIdType>
{
    protected AggregateRoot()
    {
    }
    
    protected AggregateRoot(TId id) : base(id)
    {
    }
}