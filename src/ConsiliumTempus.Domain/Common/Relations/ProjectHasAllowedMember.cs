using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Relations;

public sealed class ProjectHasAllowedMember : Entity<(ProjectId, UserId)>
{
    public override (ProjectId, UserId) Id => new(ProjectAggregateId, AllowedMembersId);
    public ProjectId ProjectAggregateId { get; init; } = null!;
    public UserId AllowedMembersId { get; init; } = null!;
}