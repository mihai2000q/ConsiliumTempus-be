using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.GetFavorites;

public sealed record GetFavoritesQuery(
    int PageSize,
    int CurrentPage
) : IRequest<ErrorOr<GetFavoritesResult>>;