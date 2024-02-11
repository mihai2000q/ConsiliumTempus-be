using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static bool AssertFromCreateCommand(
            WorkspaceAggregate workspace,
            CreateWorkspaceCommand command,
            UserAggregate user)
        {
            workspace.Name.Should().Be(command.Name);
            workspace.Description.Should().Be(command.Description);
            workspace.Memberships.Should().HaveCount(1);
            workspace.Memberships[0].Id.Should().Be((user.Id, workspace.Id));
            workspace.Memberships[0].User.Should().Be(user);
            workspace.Memberships[0].Workspace.Should().Be(workspace);
            workspace.Memberships[0].WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.Memberships[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.Memberships[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            return true;
        }

        internal static bool AssertWorkspaceId(WorkspaceId workspaceId, Guid id)
        {
            workspaceId.Should().Be(WorkspaceId.Create(id));
            return true;
        }

        internal static void AssertGetResult(GetWorkspaceResult result, WorkspaceAggregate workspace)
        {
            result.Workspace.Id.Should().Be(workspace.Id);
            result.Workspace.Name.Should().Be(workspace.Name);
            result.Workspace.Description.Should().Be(workspace.Description);
            result.Workspace.Memberships.Should().BeEquivalentTo(workspace.Memberships);
        }
    }
}