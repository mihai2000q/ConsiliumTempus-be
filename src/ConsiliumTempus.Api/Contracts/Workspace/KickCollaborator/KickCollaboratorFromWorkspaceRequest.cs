using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Workspace.KickCollaborator;

public sealed record KickCollaboratorFromWorkspaceRequest
{
    [FromRoute] public Guid Id { get; init; }
    [FromRoute] public Guid CollaboratorId { get; init; }
}