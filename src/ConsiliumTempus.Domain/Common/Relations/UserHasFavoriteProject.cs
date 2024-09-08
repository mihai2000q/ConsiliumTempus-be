using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Relations;

public sealed class UserHasFavoriteProject : Entity<(UserId, ProjectId)>
{
    private UserHasFavoriteProject() {}

    public override (UserId, ProjectId) Id => new(FavoritesId, ProjectAggregateId);
    public UserId FavoritesId { get; init; } = null!;
    public ProjectId ProjectAggregateId { get; init; } = null!;
}