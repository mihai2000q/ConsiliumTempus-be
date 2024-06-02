using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Models;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.GetFavorites;

public sealed class GetFavoritesQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository,
    IProjectRepository projectRepository)
    : IRequestHandler<GetFavoritesQuery, ErrorOr<GetFavoritesResult>>
{
    public async Task<ErrorOr<GetFavoritesResult>> Handle(GetFavoritesQuery query, CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        if (user is null) return Errors.User.NotFound;

        var (workspaces, projects) = await (
            workspaceRepository.GetFavorites(user, cancellationToken),
            projectRepository.GetFavorites(user.Id, cancellationToken));

        var favorites = new List<GetFavoritesResult.Favorite>();

        workspaces.ForEach(w => favorites.Add(new GetFavoritesResult.Favorite(
            w.Id.Value,
            w.Name,
            FavoriteType.Workspace)));
        projects.ForEach(p => favorites.Add(new GetFavoritesResult.Favorite(
            p.Id.Value,
            p.Name,
            FavoriteType.Project)));

        var paginationInfo = PaginationInfo.Create(query.PageSize, query.CurrentPage);
        favorites = favorites.Paginate(paginationInfo).ToList();

        var totalCount = workspaces.Count + projects.Count;

        return new GetFavoritesResult(favorites, totalCount);
    }
}