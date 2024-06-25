namespace ConsiliumTempus.Domain.Common.Interfaces;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}