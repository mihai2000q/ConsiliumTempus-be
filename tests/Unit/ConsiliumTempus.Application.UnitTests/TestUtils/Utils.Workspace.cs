using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

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
            workspace.Name.Value.Should().Be(command.Name);
            workspace.Description.Value.Should().BeEmpty();
            workspace.Owner.Should().Be(user);
            workspace.IsFavorite(user).Should().Be(false);
            workspace.IsPersonal.Value.Should().Be(false);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.DomainEvents.Should().BeEmpty();

            workspace.Memberships.Should().HaveCount(1);
            workspace.Memberships[0].Id.Should().Be((user.Id, workspace.Id));
            workspace.Memberships[0].User.Should().Be(user);
            workspace.Memberships[0].Workspace.Should().Be(workspace);
            workspace.Memberships[0].WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.Memberships[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.Memberships[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.Projects.Should().BeEmpty();
            workspace.Favorites.Should().BeEmpty();

            return true;
        }

        internal static void AssertFromUpdateCommand(
            WorkspaceAggregate workspace,
            UpdateWorkspaceCommand command,
            UserAggregate currentUser)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Name.Value.Should().Be(command.Name);
            workspace.IsFavorite(currentUser).Should().Be(command.IsFavorite);
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateOverviewCommand(
            WorkspaceAggregate workspace,
            UpdateOverviewWorkspaceCommand command)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Description.Value.Should().Be(command.Description);
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertWorkspace(
            GetWorkspaceResult outcome, 
            WorkspaceAggregate expected,
            UserAggregate user)
        {
            outcome.Workspace.Id.Should().Be(expected.Id);
            outcome.Workspace.Name.Should().Be(expected.Name);
            outcome.Workspace.Description.Should().Be(expected.Description);
            outcome.Workspace.IsFavorite(user).Should().Be(expected.IsFavorite(user));
            outcome.Workspace.LastActivity.Should().Be(expected.LastActivity);
            outcome.Workspace.CreatedDateTime.Should().Be(expected.CreatedDateTime);
            outcome.Workspace.UpdatedDateTime.Should().Be(expected.UpdatedDateTime);
            outcome.Workspace.Memberships.Should().BeEquivalentTo(expected.Memberships);
            outcome.Workspace.Projects.Should().BeEquivalentTo(expected.Projects);
            outcome.Workspace.Favorites.Should().BeEquivalentTo(expected.Favorites);

            outcome.CurrentUser.Should().Be(user);
        }
        
        internal static void AssertWorkspace(
            WorkspaceAggregate outcome, 
            WorkspaceAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.Description.Should().Be(expected.Description);
            outcome.LastActivity.Should().Be(expected.LastActivity);
            outcome.CreatedDateTime.Should().Be(expected.CreatedDateTime);
            outcome.UpdatedDateTime.Should().Be(expected.UpdatedDateTime);
            outcome.Memberships.Should().BeEquivalentTo(expected.Memberships);
            outcome.Projects.Should().BeEquivalentTo(expected.Projects);
            outcome.Favorites.Should().BeEquivalentTo(expected.Favorites);
        }

        internal static void AssertGetCollectionResult(
            GetCollectionWorkspaceResult result,
            GetCollectionWorkspaceQuery query,
            List<WorkspaceAggregate> workspaces,
            int workspacesCount,
            WorkspaceAggregate personalWorkspace,
            UserAggregate currentUser)
        {
            result.Workspaces.Should().BeEquivalentTo(workspaces);
            if (query.IsPersonalWorkspaceFirst)
            {
                result.Workspaces.Should().HaveElementAt(0, personalWorkspace);
            }

            result.TotalCount.Should().Be(workspacesCount);
            result.CurrentUser.Should().Be(currentUser);
        }
    }
}