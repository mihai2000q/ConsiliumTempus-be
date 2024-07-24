using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;

public sealed class RejectInvitationToWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository)
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

        workspace.RemoveInvitation(invitation);

        return new RejectInvitationToWorkspaceResult();
    }
}