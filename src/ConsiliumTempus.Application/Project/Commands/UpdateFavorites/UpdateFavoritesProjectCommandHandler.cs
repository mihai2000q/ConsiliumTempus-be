using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateFavorites;

public sealed class UpdateFavoritesProjectCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository)
    : IRequestHandler<UpdateFavoritesProjectCommand, ErrorOr<UpdateFavoritesProjectResult>>
{
    public async Task<ErrorOr<UpdateFavoritesProjectResult>> Handle(UpdateFavoritesProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        project.UpdateFavorites(command.IsFavorite, user);

        return new UpdateFavoritesProjectResult();
    }
}