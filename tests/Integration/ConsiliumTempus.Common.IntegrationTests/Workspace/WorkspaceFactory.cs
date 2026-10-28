using ConsiliumTempus.Common.IntegrationTests.Extensions;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Workspace;

public static class WorkspaceFactory
{
    public static WorkspaceAggregate Create(
        UserAggregate owner,
        Audit audit,
        string name = Constants.Workspace.Name,
        string description = Constants.Workspace.Description,
        bool isPersonal = false,
        DateTime? lastActivity = null,
        List<UserAggregate>? favorites = null)
    {
        return EntityBuilder<WorkspaceAggregate>.Empty()
            .WithProperty(nameof(WorkspaceAggregate.Id), WorkspaceId.CreateUnique())
            .WithProperty(nameof(WorkspaceAggregate.Name), Name.Create(name))
            .WithProperty(nameof(WorkspaceAggregate.Description), Description.Create(description))
            .WithProperty(nameof(WorkspaceAggregate.Owner), owner)
            .WithProperty(nameof(WorkspaceAggregate.IsPersonal), IsPersonal.Create(isPersonal))
            .WithProperty(nameof(WorkspaceAggregate.LastActivity), lastActivity ?? DateTime.UtcNow)
            .WithProperty(nameof(WorkspaceAggregate.Audit), audit)
            .WithField(nameof(WorkspaceAggregate.Favorites).ToBackingField(), favorites ?? [])
            .Build();
    }
}