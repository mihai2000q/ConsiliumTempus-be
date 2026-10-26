using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;

internal sealed class RejectInvitationToWorkspaceCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<RejectInvitationToWorkspaceCommand, ErrorOr<RejectInvitationToWorkspaceResult>>
{
    public async Task<ErrorOr<RejectInvitationToWorkspaceResult>> Handle(RejectInvitationToWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWithInvitations(
            WorkspaceId.Create(command.Id),
            cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var invitation = workspace.Invitations.SingleOrDefault(i => i.Id.Value == command.InvitationId);
        if (invitation is null) return Errors.WorkspaceInvitation.NotFound;

        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        if (user is null) return Errors.User.NotFound;
        if (invitation.Collaborator.Id != user.Id) return Errors.WorkspaceInvitation.NotInvitedUser;

        workspace.RemoveInvitation(invitation);

        return new RejectInvitationToWorkspaceResult();
    }
}