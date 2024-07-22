using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;

public sealed class AcceptInvitationToWorkspaceCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<AcceptInvitationToWorkspaceCommand, ErrorOr<AcceptInvitationToWorkspaceResult>>
{
    public async Task<ErrorOr<AcceptInvitationToWorkspaceResult>> Handle(AcceptInvitationToWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWithMembershipsAndInvitations(
            WorkspaceId.Create(command.Id),
            cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var invitation = workspace.Invitations.SingleOrDefault(i => i.Id.Value == command.InvitationId);
        if (invitation is null) return Errors.WorkspaceInvitation.NotFound;

        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        if (user is null) return Errors.User.NotFound;

        var membership = Membership.Create(
            user,
            workspace,
            WorkspaceRole.Admin);
        workspace.AddUserMembership(membership);
        workspace.RemoveInvitation(invitation);

        return new AcceptInvitationToWorkspaceResult();
    }
}