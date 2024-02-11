using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.Update;
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
            workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.Memberships.Should().HaveCount(1);
            workspace.Memberships[0].Id.Should().Be((user.Id, workspace.Id));
            workspace.Memberships[0].User.Should().Be(user);
            workspace.Memberships[0].Workspace.Should().Be(workspace);
            workspace.Memberships[0].WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.Memberships[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.Memberships[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            return true;
        }
        
        internal static void AssertFromUpdateCommand(
            WorkspaceAggregate workspace,
            UpdateWorkspaceCommand command)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Name.Should().Be(command.Name);
            workspace.Description.Should().Be(command.Description);
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        internal static bool AssertWorkspaceId(WorkspaceId workspaceId, Guid expectedId)
        {
            workspaceId.Should().Be(WorkspaceId.Create(expectedId));
            return true;
        }

        internal static void AssertDeleteResult(DeleteWorkspaceResult result, WorkspaceAggregate workspace)
        {
            AssertWorkspace(result.Workspace, workspace);
        }

        internal static void AssertWorkspace(WorkspaceAggregate outcome, WorkspaceAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.Description.Should().Be(expected.Description);
            outcome.CreatedDateTime.Should().BeCloseTo(expected.CreatedDateTime, TimeSpan.FromMinutes(1));
            outcome.UpdatedDateTime.Should().BeCloseTo(expected.UpdatedDateTime, TimeSpan.FromMinutes(1));
            outcome.Memberships.Should().BeEquivalentTo(expected.Memberships);
        }
    }
}