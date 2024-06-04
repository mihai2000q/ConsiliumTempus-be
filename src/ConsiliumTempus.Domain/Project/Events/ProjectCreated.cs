using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Project.Events;

public sealed record ProjectCreated(ProjectAggregate Project) : IDomainEvent;