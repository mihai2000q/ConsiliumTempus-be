using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.ProjectTask.Events;

public sealed record ProjectTaskDeleted(ProjectTaskAggregate ProjectTask) : IDomainEvent;