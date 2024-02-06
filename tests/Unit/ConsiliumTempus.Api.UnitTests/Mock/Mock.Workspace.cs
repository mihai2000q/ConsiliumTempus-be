using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.UnitTests.Mock;

public static partial class Mock
{
    public static class Workspace
    {
        public static WorkspaceAggregate CreateMock(string name, string description)
        {
            return WorkspaceAggregate.Create(name, description);
        }
    }
}