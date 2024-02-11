using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.UnitTests.Mock;

internal static partial class Mock
{
    internal static class Workspace
    {
        public static WorkspaceAggregate CreateMock(
            string name = "Workspace Name",
            string description = "This is the Workspace Description")
        {
            return WorkspaceAggregate.Create(name, description);
        }
    }
}