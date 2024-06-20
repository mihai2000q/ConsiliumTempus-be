using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

public sealed record GetCollaboratorsFromWorkspaceResult(
    List<UserAggregate> Collaborators);