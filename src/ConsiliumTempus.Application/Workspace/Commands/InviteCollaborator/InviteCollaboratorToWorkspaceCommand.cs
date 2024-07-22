using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;

public record InviteCollaboratorToWorkspaceCommand(
    Guid Id,
    string Email)
    : IRequest<ErrorOr<InviteCollaboratorToWorkspaceResult>>;