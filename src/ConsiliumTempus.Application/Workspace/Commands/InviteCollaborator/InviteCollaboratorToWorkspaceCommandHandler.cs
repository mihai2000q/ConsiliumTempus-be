using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;

public sealed class InviteCollaboratorToWorkspaceCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository,
    IUserRepository userRepository)
    : IRequestHandler<InviteCollaboratorToWorkspaceCommand, ErrorOr<InviteCollaboratorToWorkspaceResult>>
{
    public async Task<ErrorOr<InviteCollaboratorToWorkspaceResult>> Handle(InviteCollaboratorToWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWithCollaboratorsAndInvitations(
            WorkspaceId.Create(command.Id),
            cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var collaborator = await userRepository.GetByEmail(command.Email.ToLower(), cancellationToken);
        if (collaborator is null) return Errors.User.NotFound;

        if (workspace.Invitations.Any(i => i.Collaborator == collaborator)) 
            return Errors.WorkspaceInvitation.AlreadyInvited;
        if (workspace.Memberships.Any(m => m.User == collaborator))
            return Errors.WorkspaceInvitation.AlreadyCollaborator;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        var invitation = WorkspaceInvitation.Create(
            user,
            collaborator,
            workspace);
        workspace.AddInvitation(invitation);

        return new InviteCollaboratorToWorkspaceResult();
    }
}