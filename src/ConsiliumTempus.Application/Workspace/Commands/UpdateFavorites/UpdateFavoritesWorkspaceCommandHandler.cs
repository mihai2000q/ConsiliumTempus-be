using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;

public sealed class UpdateFavoritesWorkspaceCommandHandler(
    ICurrentUserProvider currentUserProvider, 
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<UpdateFavoritesWorkspaceCommand, ErrorOr<UpdateFavoritesWorkspaceResult>>
{
    public async Task<ErrorOr<UpdateFavoritesWorkspaceResult>> Handle(UpdateFavoritesWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.Get(WorkspaceId.Create(command.Id), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        workspace.UpdateFavorites(command.IsFavorite, user);

        return new UpdateFavoritesWorkspaceResult();
    }
}