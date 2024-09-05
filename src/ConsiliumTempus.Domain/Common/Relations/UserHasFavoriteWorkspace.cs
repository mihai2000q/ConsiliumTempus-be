using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Relations;

public sealed class UserHasFavoriteWorkspace : Entity<(UserId, WorkspaceId)>
{
    public override (UserId, WorkspaceId) Id => new(FavoritesId, WorkspaceAggregateId);
    public UserId FavoritesId { get; init; } = null!;
    public WorkspaceId WorkspaceAggregateId { get; init; } = null!;
}