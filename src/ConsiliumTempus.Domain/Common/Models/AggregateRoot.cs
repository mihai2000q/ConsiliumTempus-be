namespace ConsiliumTempus.Domain.Common.Models;

public class AggregateRoot<TId, TIdType> : Entity<TId>
    where TId : AggregateRootId<TIdType>
{
    public new AggregateRootId<TIdType> Id { get; protected set; }
    
    protected AggregateRoot()
    {
    }
    
    protected AggregateRoot(TId id)
    {
        Id = id;
    }
}