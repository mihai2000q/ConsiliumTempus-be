using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.UnitTests.Mock;

public static partial class Mock
{
    public static class Workspace
    {
        public static WorkspaceAggregate CreateMock(
            string name = "Workspace Name",
            string description = "This is the Workspace Description")
        {
            return WorkspaceAggregate.Create(name, description);
        }
    }
}