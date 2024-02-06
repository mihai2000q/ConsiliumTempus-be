using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

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
    }
}