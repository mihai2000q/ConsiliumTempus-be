using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Application.User.Queries.GetFavorites;

public sealed record GetFavoritesResult(
    List<GetFavoritesResult.Favorite> Favorites,
    int TotalCount)
{
    public sealed record Favorite(
        Guid Id,
        Name Name,
        FavoriteType FavoriteType);
}