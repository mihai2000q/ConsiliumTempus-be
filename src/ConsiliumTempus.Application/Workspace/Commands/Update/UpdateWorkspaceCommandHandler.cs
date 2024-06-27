using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Update;

public sealed class UpdateWorkspaceCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<UpdateWorkspaceCommand, ErrorOr<UpdateWorkspaceResult>>
{
    public async Task<ErrorOr<UpdateWorkspaceResult>> Handle(UpdateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.Get(WorkspaceId.Create(command.Id), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        workspace.Update(
            Name.Create(command.Name),
            command.IsFavorite,
            user);

        return new UpdateWorkspaceResult();
    }
}