using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.KickCollaborator;

public sealed record KickCollaboratorFromWorkspaceCommand(
    Guid Id,
    Guid CollaboratorId)
    : IRequest<ErrorOr<KickCollaboratorFromWorkspaceResult>>;