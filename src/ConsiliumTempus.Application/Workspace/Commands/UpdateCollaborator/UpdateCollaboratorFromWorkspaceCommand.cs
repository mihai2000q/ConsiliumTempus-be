using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;

public sealed record UpdateCollaboratorFromWorkspaceCommand(
    Guid Id,
    Guid CollaboratorId,
    string WorkspaceRole)
    : IRequest<ErrorOr<UpdateCollaboratorFromWorkspaceResult>>;