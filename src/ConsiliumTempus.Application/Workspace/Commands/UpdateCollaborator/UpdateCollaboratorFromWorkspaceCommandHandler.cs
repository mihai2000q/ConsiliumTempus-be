using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;

public sealed class UpdateCollaboratorFromWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<UpdateCollaboratorFromWorkspaceCommand, ErrorOr<UpdateCollaboratorFromWorkspaceResult>>
{
    public async Task<ErrorOr<UpdateCollaboratorFromWorkspaceResult>> Handle(
        UpdateCollaboratorFromWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWithCollaborators(WorkspaceId.Create(command.Id), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var collaboratorMembership = workspace.Memberships
            .SingleOrDefault(m => m.User.Id.Value == command.CollaboratorId);
        if (collaboratorMembership is null) return Errors.Workspace.CollaboratorNotFound;

        var workspaceRole = WorkspaceRole.FromName(command.WorkspaceRole.Capitalize())!;
        collaboratorMembership.Update(workspaceRole);
        workspace.RefreshActivity();

        return new UpdateCollaboratorFromWorkspaceResult();
    }
}