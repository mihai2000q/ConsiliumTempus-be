using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

public sealed record GetCollaboratorsFromWorkspaceResult(
    List<Membership> Collaborators,
    int TotalCount);