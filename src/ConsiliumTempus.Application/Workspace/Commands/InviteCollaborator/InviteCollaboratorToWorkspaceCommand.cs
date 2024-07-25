using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;

public sealed record InviteCollaboratorToWorkspaceCommand(
    Guid Id,
    string Email)
    : IRequest<ErrorOr<InviteCollaboratorToWorkspaceResult>>;