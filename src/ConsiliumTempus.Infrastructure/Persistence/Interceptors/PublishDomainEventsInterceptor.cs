using ConsiliumTempus.Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ConsiliumTempus.Infrastructure.Persistence.Interceptors;

public sealed class PublishDomainEventInterceptor(IPublisher mediator) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        PublishDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        await PublishDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext? dbContext)
    {
        if (dbContext is null) return;

        List<IHasDomainEvents> entitiesWithDomainEvents;
        // loop for domain events within domain events
        do
        {
            // Get hold of all the various entities
            entitiesWithDomainEvents = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
                .Where(entry => entry.Entity.DomainEvents.Any())
                .Select(entry => entry.Entity)
                .ToList();

            // Get hold of all the various domain events
            var domainEvents = entitiesWithDomainEvents
                .SelectMany(entry => entry.DomainEvents)
                .ToList();

            // Clear domain events
            entitiesWithDomainEvents.ForEach(entity => entity.ClearDomainEvents());

            // Publish domain events
            foreach (var domainEvent in domainEvents) await mediator.Publish(domainEvent);
        } while (entitiesWithDomainEvents.Count > 0);
    }
}