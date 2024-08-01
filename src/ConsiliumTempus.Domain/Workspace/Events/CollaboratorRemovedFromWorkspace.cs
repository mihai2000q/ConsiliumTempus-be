using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Workspace.Events;

public sealed record CollaboratorRemovedFromWorkspace(
    WorkspaceAggregate Workspace, 
    UserAggregate Collaborator) 
    : IDomainEvent;