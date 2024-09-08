using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.ProjectTask.Events;

public sealed record ProjectTaskCreated(ProjectTaskAggregate ProjectTask) : IDomainEvent;