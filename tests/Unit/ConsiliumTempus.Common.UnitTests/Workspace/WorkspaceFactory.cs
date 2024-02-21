using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceFactory
{
    public static WorkspaceAggregate Create(
        string name = Constants.Workspace.Name,
        string description = Constants.Workspace.Description)
    {
        return WorkspaceAggregate.Create(
            Name.Create(name), 
            Description.Create(description));
    }
    
    public static List<WorkspaceAggregate> CreateList()
    {
        return Enumerable.Repeat(0, 5).Select(_ => Create()).ToList();
    }
}