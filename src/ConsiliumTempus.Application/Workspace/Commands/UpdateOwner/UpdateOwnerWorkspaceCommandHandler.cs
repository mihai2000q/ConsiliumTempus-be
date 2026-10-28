using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;

internal sealed class UpdateOwnerWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<UpdateOwnerWorkspaceCommand, ErrorOr<UpdateOwnerWorkspaceResult>>
{
    public async Task<ErrorOr<UpdateOwnerWorkspaceResult>> Handle(UpdateOwnerWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWithCollaborators(WorkspaceId.Create(command.Id), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;
        if (workspace.IsPersonal.Value) return Errors.Workspace.UpdatePersonalWorkspaceOwner;

        var ownerMembership = workspace.Memberships
            .SingleOrDefault(m => m.User.Id.Value == command.OwnerId);
        if (ownerMembership is null) return Errors.Workspace.CollaboratorNotFound;

        if (ownerMembership.WorkspaceRole != WorkspaceRole.Admin) ownerMembership.Update(WorkspaceRole.Admin);
        workspace.UpdateOwner(ownerMembership.User);
        workspace.RefreshActivity();

        return new UpdateOwnerWorkspaceResult();
    }
}