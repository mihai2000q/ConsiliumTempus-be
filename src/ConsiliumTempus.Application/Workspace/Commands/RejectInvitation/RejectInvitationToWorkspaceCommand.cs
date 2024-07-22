using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;

public sealed record RejectInvitationToWorkspaceCommand(
    Guid Id,
    Guid InvitationId)
    : IRequest<ErrorOr<RejectInvitationToWorkspaceResult>>;