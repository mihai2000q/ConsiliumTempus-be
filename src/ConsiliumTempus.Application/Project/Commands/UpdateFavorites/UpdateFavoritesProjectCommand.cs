using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateFavorites;

public sealed record UpdateFavoritesProjectCommand(
    Guid Id,
    bool IsFavorite)
    : IRequest<ErrorOr<UpdateFavoritesProjectResult>>;