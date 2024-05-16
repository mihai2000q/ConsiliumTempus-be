using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static void AssertFromCreateCommand(
            CreateProjectTaskCommand command,
            Domain.ProjectSprint.Entities.ProjectStage stage,
            UserAggregate user)
        {
            var task = command.OnTop ? stage.Tasks[0] : stage.Tasks[^1];
            
            task.Id.Value.Should().NotBeEmpty();
            task.Name.Value.Should().Be(command.Name);
            task.Description.Value.Should().Be(string.Empty);
            task.CustomOrderPosition.Value.Should().Be(command.OnTop ? 0 : stage.Tasks.Count - 1);
            task.IsCompleted.Value.Should().Be(false);
            task.CreatedBy.Should().Be(user);
            task.Asignee.Should().BeNull();
            task.Reviewer.Should().BeNull();
            task.DueDate.Should().BeNull();
            task.EstimatedDuration.Should().BeNull();
            task.Stage.Should().Be(stage);
            task.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            task.Comments.Should().BeEmpty();
            task.DomainEvents.Should().BeEmpty();
            
            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

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
            var customOrderPosition = 0;
            stage.Tasks.Should().AllSatisfy(t =>
                t.CustomOrderPosition.Value.Should().Be(customOrderPosition++));

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
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
            outcome.Asignee.Should().Be(expected.Asignee);
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