using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;

namespace ConsiliumTempus.Common.IntegrationTests.Project;

public static class ProjectFactory
{
    public static ProjectAggregate Create(
        WorkspaceAggregate workspace,
        UserAggregate owner,
        string name = Constants.Project.Name,
        string description = Constants.Project.Description,
        bool isPrivate = false,
        ProjectLifecycle lifecycle = ProjectLifecycle.Active,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null,
        List<UserAggregate>? favorites = null,
        List<UserAggregate>? allowedMembers = null)
    {
        return EntityBuilder<ProjectAggregate>.Empty()
            .WithProperty(nameof(ProjectAggregate.Id), ProjectId.CreateUnique())
            .WithProperty(nameof(ProjectAggregate.Name), Name.Create(name))
            .WithProperty(nameof(ProjectAggregate.Description), Description.Create(description))
            .WithProperty(nameof(ProjectAggregate.IsPrivate), IsPrivate.Create(isPrivate))
            .WithProperty(nameof(ProjectAggregate.Owner), owner)
            .WithProperty(nameof(ProjectAggregate.Lifecycle), lifecycle)
            .WithProperty(nameof(ProjectAggregate.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(ProjectAggregate.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(ProjectAggregate.Workspace), workspace)
            .WithField(nameof(ProjectAggregate.Favorites).ToBackingField(), favorites ?? [])
            .WithField(nameof(ProjectAggregate.AllowedMembers).ToBackingField(), allowedMembers ?? [])
            .Build();
    }
}