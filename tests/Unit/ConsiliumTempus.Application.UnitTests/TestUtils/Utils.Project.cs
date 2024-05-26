using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static bool AssertFromCreateCommand(
            ProjectAggregate project,
            CreateProjectCommand command,
            WorkspaceAggregate workspace,
            UserAggregate user)
        {
            project.Id.Value.Should().NotBeEmpty();
            project.Name.Value.Should().Be(command.Name);
            project.Description.Value.Should().BeEmpty();
            project.IsPrivate.Value.Should().Be(command.IsPrivate);
            project.IsFavorite.Value.Should().Be(false);
            project.Sprints.Should().BeEmpty();
            project.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.Should().Be(workspace);

            project.DomainEvents.Should().HaveCount(1);
            project.DomainEvents[0].Should().BeOfType<ProjectCreated>();
            ((ProjectCreated)project.DomainEvents[0]).Project.Should().Be(project);
            ((ProjectCreated)project.DomainEvents[0]).User.Should().Be(user);

            return true;
        }

        internal static bool AssertFromDeleteCommand(
            ProjectAggregate project,
            DeleteProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);

            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            return true;
        }

        internal static void AssertFromUpdateCommand(
            ProjectAggregate project,
            UpdateProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);
            project.Name.Value.Should().Be(command.Name);
            project.IsFavorite.Value.Should().Be(command.IsFavorite);
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
        
        internal static void AssertFromUpdateOverviewCommand(
            ProjectAggregate project,
            UpdateOverviewProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);
            project.Description.Value.Should().Be(command.Description);
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertProject(
            ProjectAggregate outcome,
            ProjectAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.Description.Should().Be(expected.Description);
            outcome.IsFavorite.Should().Be(expected.IsFavorite);
            outcome.IsPrivate.Should().Be(expected.IsPrivate);
            outcome.LastActivity.Should().Be(expected.LastActivity);
            outcome.CreatedDateTime.Should().Be(expected.CreatedDateTime);
            outcome.UpdatedDateTime.Should().Be(expected.UpdatedDateTime);
            outcome.Workspace.Should().Be(expected.Workspace);
            outcome.Sprints.Should().BeEquivalentTo(expected.Sprints);
        }

        internal static void AssertProjectOverview(
            GetOverviewProjectResult outcome,
            ProjectAggregate project)
        {
            outcome.Description.Should().Be(project.Description);
        }

        internal static bool AssertGetCollectionProjectFilters(
            IReadOnlyList<IFilter<ProjectAggregate>> filters,
            GetCollectionProjectQuery query)
        {
            filters.OfType<Filters.Project.WorkspaceFilter>().Single().Value.Should().Be(
                query.WorkspaceId.IfNotNull(WorkspaceId.Create));
            filters.OfType<Filters.Project.NameFilter>().Single().Value.Should().Be(query.Name);
            filters.OfType<Filters.Project.IsFavoriteFilter>().Single().Value.Should().Be(query.IsFavorite);
            filters.OfType<Filters.Project.IsPrivateFilter>().Single().Value.Should().Be(query.IsPrivate);

            return true;
        }
    }
}