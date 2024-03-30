using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Workspace;

public static class WorkspaceFactory
{
    public static WorkspaceAggregate Create(
        string name = Constants.Workspace.Name,
        string description = Constants.Workspace.Description,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        var workspace = DomainFactory.GetObjectInstance<WorkspaceAggregate>();
        
        DomainFactory.SetProperty(ref workspace, nameof(workspace.Id), WorkspaceId.CreateUnique());
        DomainFactory.SetProperty(ref workspace, nameof(workspace.Name), Name.Create(name));
        DomainFactory.SetProperty(ref workspace, nameof(workspace.Description), Description.Create(description));
        DomainFactory.SetProperty(ref workspace, nameof(workspace.CreatedDateTime), createdDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref workspace, nameof(workspace.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow);

        return workspace;
    }
}