using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectSprint
    {
        internal static void AssertAddStageCommand(
            ProjectSprintAggregate sprint,
            AddStageToProjectSprintCommand command)
        {
            var projectStage = command.OnTop ? sprint.Stages[0] : sprint.Stages[^1];

            projectStage.Id.Value.Should().NotBeEmpty();
            projectStage.Name.Value.Should().Be(command.Name);
            projectStage.CustomOrderPosition.Value.Should().Be(command.OnTop ? 0 : sprint.Stages.Count - 1);
            projectStage.Sprint.Should().Be(sprint);
            projectStage.Tasks.Should().BeEmpty();
            projectStage.DomainEvents.Should().BeEmpty();

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());

            var count = 0;
            sprint.Stages.Should().AllSatisfy(stage => stage.CustomOrderPosition.Value.Should().Be(count++));
        }

        internal static bool AssertFromCreateCommand(
            ProjectSprintAggregate projectSprint,
            CreateProjectSprintCommand command,
            ProjectAggregate project)
        {
            projectSprint.Id.Value.Should().NotBeEmpty();
            projectSprint.Name.Value.Should().Be(command.Name);
            projectSprint.StartDate.Should().Be(command.StartDate);
            projectSprint.EndDate.Should().Be(command.EndDate);
            if (command.KeepPreviousStages && project.Sprints.Count != 0)
                projectSprint.Stages.Should().BeEquivalentTo(project.Sprints[^1].Stages);
            else
                projectSprint.Stages.Should().BeEmpty();
            projectSprint.DomainEvents.Should().BeEmpty();

            projectSprint.Project.Should().Be(project);
            projectSprint.Project.LastActivity
                .Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());

            projectSprint.Project.Workspace.LastActivity
                .Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());

            return true;
        }

        internal static bool AssertFromDeleteCommand(
            ProjectSprintAggregate sprint,
            DeleteProjectSprintCommand command)
        {
            sprint.Id.Value.Should().Be(command.Id);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());

            return true;
        }

        internal static void AssertRemoveStageCommand(
            Domain.ProjectSprint.Entities.ProjectStage stage,
            RemoveStageFromProjectSprintCommand command)
        {
            stage.Id.Value.Should().Be(command.StageId);

            var sprint = stage.Sprint;

            sprint.Stages.Should().NotContain(stage);
            var customOrderPosition = 0;
            sprint.Stages.Should().AllSatisfy(s => 
                s.CustomOrderPosition.Value.Should().Be(customOrderPosition++));

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
        }

        internal static void AssertFromUpdateCommand(
            ProjectSprintAggregate sprint,
            UpdateProjectSprintCommand command)
        {
            sprint.Id.Value.Should().Be(command.Id);
            sprint.Name.Value.Should().Be(command.Name);
            sprint.StartDate.Should().Be(command.StartDate);
            sprint.EndDate.Should().Be(command.EndDate);
            sprint.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
        }

        internal static void AssertUpdateStageCommand(
            Domain.ProjectSprint.Entities.ProjectStage stage,
            UpdateStageFromProjectSprintCommand command)
        {
            stage.Id.Value.Should().Be(command.StageId);
            stage.Name.Value.Should().Be(command.Name);

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
        }

        internal static void AssertProjectSprint(
            ProjectSprintAggregate outcome,
            ProjectSprintAggregate expected)
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
    }
}