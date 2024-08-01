using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.KickCollaborator;

public sealed class KickCollaboratorFromWorkspaceCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<KickCollaboratorFromWorkspaceCommand, ErrorOr<KickCollaboratorFromWorkspaceResult>>
{
    public async Task<ErrorOr<KickCollaboratorFromWorkspaceResult>> Handle(KickCollaboratorFromWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWithCollaborators(
            WorkspaceId.Create(command.Id),
            cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var collaboratorMembership = workspace.Memberships
            .SingleOrDefault(m => m.User.Id.Value == command.CollaboratorId);
        if (collaboratorMembership is null) return Errors.Workspace.CollaboratorNotFound;
        if (collaboratorMembership.User == workspace.Owner) return Errors.Workspace.KickOwner;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);
        if (collaboratorMembership.User == user) return Errors.Workspace.KickYourself;

        workspace.RemoveUserMembership(collaboratorMembership);
        workspace.RefreshActivity();

        return new KickCollaboratorFromWorkspaceResult();
    }
}