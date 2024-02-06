using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

public static partial class Utils
{
    public static class Workspace
    {
        public static bool AssertFromCreateCommand(
            WorkspaceAggregate workspace,
            CreateWorkspaceCommand command,
            UserAggregate user)
        {
            workspace.Name.Should().Be(command.Name);
            workspace.Description.Should().Be(command.Description);
            workspace.Users.Should().HaveCount(1);
            workspace.Users[0].Should().Be(user);
            return true;
        }
        
        public static bool AssertWorkspaceId(WorkspaceId workspaceId, string id)
        {
            workspaceId.Should().Be(WorkspaceId.Create(id));
            return true;
        }

        public static void AssertGetResult(GetWorkspaceResult result, WorkspaceAggregate workspace)
        {
            result.Workspace.Id.Should().Be(workspace.Id);
            result.Workspace.Name.Should().Be(workspace.Name);
            result.Workspace.Description.Should().Be(workspace.Description);
            result.Workspace.Users.Should().BeEquivalentTo(workspace.Users);
        }
    }
}