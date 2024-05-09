using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
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
            workspace.Description.Value.Should().Be(command.Description);
            workspace.Owner.Should().Be(user);
            workspace.IsPersonal.Value.Should().Be(false);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.DomainEvents.Should().BeEmpty();

            workspace.Memberships.Should().HaveCount(1);
            workspace.Memberships[0].Id.Should().Be((user.Id, workspace.Id));
            workspace.Memberships[0].User.Should().Be(user);
            workspace.Memberships[0].Workspace.Should().Be(workspace);
            workspace.Memberships[0].WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.Memberships[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.Memberships[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            workspace.Projects.Should().BeEmpty();
            return true;
        }

        internal static void AssertFromUpdateCommand(
            WorkspaceAggregate workspace,
            UpdateWorkspaceCommand command)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Name.Value.Should().Be(command.Name);
            workspace.Description.Value.Should().Be(command.Description);
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        internal static bool AssertGetCollectionFilters(
            IEnumerable<IFilter<WorkspaceAggregate>> filters,
            GetCollectionWorkspaceQuery query)
        {
            filters.OfType<Filters.Workspace.NameFilter>().Single().Value.Should().Be(query.Name);

            return true;
        }

        internal static void AssertWorkspace(WorkspaceAggregate outcome, WorkspaceAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.Description.Should().Be(expected.Description);
            outcome.LastActivity.Should().Be(expected.LastActivity);
            outcome.CreatedDateTime.Should().Be(expected.CreatedDateTime);
            outcome.UpdatedDateTime.Should().Be(expected.UpdatedDateTime);
            outcome.Memberships.Should().BeEquivalentTo(expected.Memberships);
            outcome.Projects.Should().BeEquivalentTo(expected.Projects);
        }
    }
}