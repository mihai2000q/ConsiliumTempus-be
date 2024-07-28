using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;

public sealed record UpdateFavoritesWorkspaceCommand(
    Guid Id,
    bool IsFavorite)
    : IRequest<ErrorOr<UpdateFavoritesWorkspaceResult>>;