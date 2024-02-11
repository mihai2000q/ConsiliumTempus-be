using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Mock;

internal static partial class Mock
{
    internal static class Workspace
    {
        internal static WorkspaceAggregate CreateMock(
            string name = "Workspace Name",
            string description = "This is the Workspace Description")
        {
            return WorkspaceAggregate.Create(name, description);
        }
        
        internal static List<WorkspaceAggregate> CreateListMock()
        {
            return Enumerable.Repeat(0, 5).Select(_ => CreateMock()).ToList();
        }
    }
}