using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Project.Events;

public sealed record ProjectDeleted(ProjectAggregate Project) : IDomainEvent;