using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static void AssertFromCreateCommand(
            CreateProjectTaskCommand command,
            ProjectStage stage,
            UserAggregate user)
        {
            var task = command.OnTop ? stage.Tasks[0] : stage.Tasks[^1];

            task.Id.Value.Should().NotBeEmpty();
            task.Name.Value.Should().Be(command.Name);
            task.Description.Value.Should().Be(string.Empty);
            task.CustomOrderPosition.Value.Should().Be(command.OnTop ? 0 : stage.Tasks.Count - 1);
            task.IsCompleted.Value.Should().Be(false);
            task.CreatedBy.Should().Be(user);
            task.Assignee.Should().BeNull();
            task.Reviewer.Should().BeNull();
            task.DueDate.Should().BeNull();
            task.EstimatedDuration.Should().BeNull();
            task.Stage.Should().Be(stage);
            task.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Comments.Should().BeEmpty();
            task.DomainEvents.Should().BeEmpty();

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            var count = 0;
            stage.Tasks.Should().AllSatisfy(t => t.CustomOrderPosition.Value.Should().Be(count++));
        }

        internal static void AssertFromDeleteCommand(
            ProjectTaskAggregate task,
            DeleteProjectTaskCommand command)
        {
            task.Id.Value.Should().Be(command.Id);

            var stage = task.Stage;

            stage.Tasks.Should().NotContain(task);
            stage.Tasks.ShouldBeOrdered();

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateCommand(
            ProjectTaskAggregate task,
            UpdateProjectTaskCommand command,
            UserAggregate assignee)
        {
            task.Name.Value.Should().Be(command.Name);
            task.IsCompleted.Value.Should().Be(command.IsCompleted);
            task.Assignee.Should().Be(command.AssigneeId is null ? null : assignee);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromMoveCommandToAnotherStage(
            ProjectTaskAggregate task,
            MoveProjectTaskCommand command,
            List<ProjectStage> stages)
        {
            var overStage = stages.Single(s => s.Id.Value == command.OverId);

            task.Id.Value.Should().Be(command.Id);
            task.CustomOrderPosition.Value.Should().Be(0);
            task.Stage.Should().Be(overStage);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            overStage.Tasks.Should().Contain(task);
            overStage.Tasks[0].Should().Be(task);
            overStage.Tasks.ShouldBeOrdered();

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromMoveCommandWithinStage(
            ProjectTaskAggregate task,
            MoveProjectTaskCommand command,
            int expectedCustomOrderPosition)
        {
            task.Id.Value.Should().Be(command.Id);
            task.CustomOrderPosition.Value.Should().Be(expectedCustomOrderPosition);
            task.Stage.Tasks.ShouldBeOrdered();
            task.Stage.Tasks.Should().ContainSingle(t => t.Id.Value == command.OverId);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromMoveCommandOverTaskInAnotherStage(
            ProjectTaskAggregate task,
            MoveProjectTaskCommand command,
            List<ProjectStage> stages,
            int expectedCustomOrderPosition)
        {
            var overStage = stages
                .SelectMany(s => s.Tasks)
                .Single(s => s.Id.Value == command.OverId)
                .Stage;

            task.Id.Value.Should().Be(command.Id);
            task.CustomOrderPosition.Value.Should().Be(expectedCustomOrderPosition);
            task.Stage.Should().Be(overStage);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            stages.Should().AllSatisfy(s => s.Tasks.ShouldBeOrdered());

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateOverviewCommand(
            ProjectTaskAggregate task,
            UpdateOverviewProjectTaskCommand command,
            UserAggregate assignee)
        {
            task.Name.Value.Should().Be(command.Name);
            task.Description.Value.Should().Be(command.Description);
            task.IsCompleted.Value.Should().Be(command.IsCompleted);
            task.Assignee.Should().Be(command.AssigneeId is null ? null : assignee);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertProjectTask(
            ProjectTaskAggregate outcome,
            ProjectTaskAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.Description.Should().Be(expected.Description);
            outcome.CustomOrderPosition.Should().Be(expected.CustomOrderPosition);
            outcome.IsCompleted.Should().Be(expected.IsCompleted);
            outcome.CreatedBy.Should().Be(expected.CreatedBy);
            outcome.Assignee.Should().Be(expected.Assignee);
            outcome.Reviewer.Should().Be(expected.Reviewer);
            outcome.DueDate.Should().Be(expected.DueDate);
            outcome.EstimatedDuration.Should().Be(expected.EstimatedDuration);
            outcome.CreatedDateTime.Should().Be(expected.CreatedDateTime);
            outcome.UpdatedDateTime.Should().Be(expected.UpdatedDateTime);
            outcome.Stage.Should().Be(expected.Stage);
            outcome.Comments.Should().BeEquivalentTo(expected.Comments);
        }
    }
}