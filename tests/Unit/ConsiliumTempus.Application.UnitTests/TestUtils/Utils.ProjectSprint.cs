using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectSprint
    {
        internal static void AssertFromAddStageCommand(
            ProjectSprintAggregate sprint,
            AddStageToProjectSprintCommand command,
            UserAggregate createdBy,
            UserAggregate sprintCreatedBy)
        {
            var projectStage = command.OnTop ? sprint.Stages[0] : sprint.Stages[^1];

            projectStage.Id.Value.Should().NotBeEmpty();
            projectStage.Name.Value.Should().Be(command.Name);
            projectStage.CustomOrderPosition.Value.Should().Be(command.OnTop ? 0 : sprint.Stages.Count - 1);
            projectStage.Sprint.Should().Be(sprint);
            projectStage.Audit.ShouldBeCreated(createdBy);
            projectStage.Tasks.Should().BeEmpty();
            projectStage.DomainEvents.Should().BeEmpty();

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            sprint.Stages.ShouldBeOrdered();
            sprint.Stages.Should().AllSatisfy(stage =>
            {
                stage.Audit.UpdatedBy.Should().Be(stage == projectStage ? createdBy : sprintCreatedBy);
            });
        }

        internal static bool AssertFromCreateCommand(
            ProjectSprintAggregate projectSprint,
            CreateProjectSprintCommand command,
            ProjectAggregate project,
            UserAggregate createdBy,
            DateOnly? previousSprintEndDate)
        {
            projectSprint.Id.Value.Should().NotBeEmpty();
            projectSprint.Name.Value.Should().Be(command.Name);
            projectSprint.StartDate.Should().Be(command.StartDate);
            projectSprint.EndDate.Should().Be(command.EndDate);
            projectSprint.Audit.ShouldBeCreated(createdBy);
            projectSprint.DomainEvents.Should().BeEmpty();
            projectSprint.Project.Should().Be(project);

            if (command.KeepPreviousStages)
            {
                if (project.Sprints.IsNotEmpty())
                {
                    projectSprint.Stages.Should().HaveSameCount(project.Sprints[0].Stages);
                    projectSprint.Stages
                        .OrderBy(s => s.CustomOrderPosition.Value)
                        .Zip(project.Sprints[0].Stages.OrderBy(s => s.CustomOrderPosition.Value))
                        .Should().AllSatisfy((x) =>
                        {
                            var (newStage, stage) = x;
                            newStage.Name.Should().Be(stage.Name);
                            newStage.CustomOrderPosition.Should().Be(stage.CustomOrderPosition);
                            newStage.Sprint.Should().Be(projectSprint);
                            newStage.Audit.ShouldBeCreated(createdBy);
                        });
                }
                else
                    projectSprint.Stages.Should().BeEmpty();
            }
            else
            {
                projectSprint.Stages.Should().HaveCount(1);
                projectSprint.Stages[0].Id.Value.Should().NotBeEmpty();
                projectSprint.Stages[0].Name.Value.Should().Be(Constants.ProjectStage.Names[0]);
                projectSprint.Stages[0].CustomOrderPosition.Value.Should().Be(0);
                projectSprint.Stages[0].Sprint.Should().Be(projectSprint);
                projectSprint.Stages[0].Tasks.Should().BeEmpty();
            }

            if (project.Sprints.IsNotEmpty())
            {
                if (previousSprintEndDate is null)
                    project.Sprints[0].EndDate.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
                else
                    project.Sprints[0].EndDate.Should().Be(previousSprintEndDate);
            }

            if (command.ProjectStatus is not null)
            {
                project.LatestStatus.Should().NotBeNull();
                project.LatestStatus!.Title.Value.Should().Be(command.ProjectStatus.Title);
                project.LatestStatus!.Status.ToString().ToLower().Should().Be(command.ProjectStatus.Status.ToLower());
                project.LatestStatus!.Description.Value.Should().Be(command.ProjectStatus.Description);
            }

            projectSprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            projectSprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            return true;
        }

        internal static bool AssertFromDeleteCommand(
            ProjectSprintAggregate sprint,
            DeleteProjectSprintCommand command)
        {
            sprint.Id.Value.Should().Be(command.Id);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            return true;
        }
        
        internal static void AssertFromMoveStageCommand(
            Domain.ProjectSprint.Entities.ProjectStage stage,
            MoveStageFromProjectSprintCommand command,
            UserAggregate updatedBy,
            int newCustomOrderPosition)
        {
            stage.Id.Value.Should().Be(command.StageId);
            stage.CustomOrderPosition.Value.Should().Be(newCustomOrderPosition);
            stage.Audit.ShouldBeUpdated(updatedBy);

            var sprint = stage.Sprint;

            sprint.Stages.ShouldBeOrdered();
            
            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromRemoveStageCommand(
            Domain.ProjectSprint.Entities.ProjectStage stage,
            RemoveStageFromProjectSprintCommand command)
        {
            stage.Id.Value.Should().Be(command.StageId);

            var sprint = stage.Sprint;

            sprint.Stages.Should().NotContain(stage);
            sprint.Stages.ShouldBeOrdered();

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateCommand(
            ProjectSprintAggregate sprint,
            UpdateProjectSprintCommand command,
            UserAggregate updatedBy)
        {
            sprint.Id.Value.Should().Be(command.Id);
            sprint.Name.Value.Should().Be(command.Name);
            sprint.StartDate.Should().Be(command.StartDate);
            sprint.EndDate.Should().Be(command.EndDate);

            sprint.Audit.ShouldBeUpdated(updatedBy);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateStageCommand(
            Domain.ProjectSprint.Entities.ProjectStage stage,
            UpdateStageFromProjectSprintCommand command,
            UserAggregate updatedBy)
        {
            stage.Id.Value.Should().Be(command.StageId);
            stage.Name.Value.Should().Be(command.Name);

            stage.Audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            stage.Audit.UpdatedBy.Should().Be(updatedBy);

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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
            outcome.Audit.Should().Be(expected.Audit);
            outcome.Stages.Should().BeEquivalentTo(expected.Stages);
        }
    }
}