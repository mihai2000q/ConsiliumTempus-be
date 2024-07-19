namespace ConsiliumTempus.Api.Contracts.Workspace.UpdateFavorites;

public sealed record UpdateFavoritesWorkspaceRequest(
    Guid Id,
    bool IsFavorite);