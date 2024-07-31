using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;

public sealed class UpdateOwnerWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<UpdateOwnerWorkspaceCommand, ErrorOr<UpdateOwnerWorkspaceResult>>
{
    public async Task<ErrorOr<UpdateOwnerWorkspaceResult>> Handle(UpdateOwnerWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWithCollaborators(WorkspaceId.Create(command.Id), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var owner = workspace.Memberships
            .SingleOrDefault(m => m.User.Id.Value == command.OwnerId)
            ?.User;
        if (owner is null) return Errors.Workspace.CollaboratorNotFound;

        workspace.UpdateOwner(owner);
        workspace.RefreshUpdatedDateTime();
        workspace.RefreshActivity();

        return new UpdateOwnerWorkspaceResult();
    }
}