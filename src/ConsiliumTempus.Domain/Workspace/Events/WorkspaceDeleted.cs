using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Workspace.Events;

public sealed record WorkspaceDeleted(WorkspaceAggregate Workspace) : IDomainEvent;