namespace ConsiliumTempus.Api.Contracts.Project.UpdateFavorites;

public sealed record UpdateFavoritesProjectRequest(
    Guid Id,
    bool IsFavorite);