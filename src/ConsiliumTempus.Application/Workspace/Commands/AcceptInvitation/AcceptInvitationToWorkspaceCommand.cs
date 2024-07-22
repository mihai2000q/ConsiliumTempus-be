using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;

public sealed record AcceptInvitationToWorkspaceCommand(
    Guid Id,
    Guid InvitationId)
    : IRequest<ErrorOr<AcceptInvitationToWorkspaceResult>>;