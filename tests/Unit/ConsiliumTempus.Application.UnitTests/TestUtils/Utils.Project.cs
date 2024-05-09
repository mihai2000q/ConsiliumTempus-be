using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Update;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using FluentAssertions.Extensions;

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
            project.Description.Value.Should().Be(command.Description);
            project.IsPrivate.Value.Should().Be(command.IsPrivate);
            project.IsFavorite.Value.Should().Be(false);
            project.Sprints.Should().BeEmpty();
            project.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            project.Workspace.Should().Be(workspace);

            project.DomainEvents.Should().HaveCount(1);
            project.DomainEvents[0].Should().BeOfType<ProjectCreated>();
            ((ProjectCreated)project.DomainEvents[0]).Project.Should().Be(project);
            ((ProjectCreated)project.DomainEvents[0]).User.Should().Be(user);

            return true;
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

    internal static class ProjectSprint
    {
        internal static bool AssertFromCreateCommand(
            Domain.Project.Entities.ProjectSprint projectSprint,
            CreateProjectSprintCommand command,
            ProjectAggregate project)
        {
            projectSprint.Id.Value.Should().NotBeEmpty();
            projectSprint.Name.Value.Should().Be(command.Name);
            projectSprint.StartDate.Should().Be(command.StartDate);
            projectSprint.EndDate.Should().Be(command.EndDate);

            projectSprint.Project.Should().Be(project);
            projectSprint.Project
                .LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

            projectSprint.Project.Workspace
                .LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

            return true;
        }
        
        internal static void AssertProjectSprint(
            Domain.Project.Entities.ProjectSprint outcome,
            Domain.Project.Entities.ProjectSprint expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.StartDate.Should().Be(expected.StartDate);
            outcome.EndDate.Should().Be(expected.EndDate);
            outcome.Project.Should().Be(expected.Project);
            outcome.CreatedDateTime.Should().Be(expected.CreatedDateTime);
            outcome.UpdatedDateTime.Should().Be(expected.UpdatedDateTime);
            outcome.Stages.Should().BeEquivalentTo(expected.Stages);
        }
        
        internal static void AssertFromUpdateCommand(
            Domain.Project.Entities.ProjectSprint sprint,
            UpdateProjectSprintCommand command)
        {
            sprint.Id.Value.Should().Be(command.Id);
            sprint.Name.Value.Should().Be(command.Name);
            sprint.StartDate.Should().Be(command.StartDate);
            sprint.EndDate.Should().Be(command.EndDate);
            sprint.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }
    }
}