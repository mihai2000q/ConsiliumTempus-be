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
        internal static bool AssertFromCreateCommand(
            ProjectTaskAggregate task,
            CreateProjectTaskCommand command,
            Domain.Project.Entities.ProjectStage stage,
            UserAggregate user)
        {
            task.Id.Value.Should().NotBeEmpty();
            task.Name.Value.Should().Be(command.Name);
            task.Description.Value.Should().Be(string.Empty);
            task.CustomOrderPosition.Value.Should().Be(stage.Tasks.Count);
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

            return true;
        }
        
        internal static void AssertFromDeleteCommand(
            ProjectTaskAggregate task,
            Domain.Project.Entities.ProjectStage stage,
            DeleteProjectTaskCommand command)
        {
            task.Id.Value.Should().Be(command.Id);
            stage.Id.Value.Should().Be(command.StageId);
            
            stage.Tasks.Should().NotContain(task);
            var customOrderPosition = 0;
            stage.Tasks.Should().AllSatisfy(t =>
                t.CustomOrderPosition.Value.Should().Be(customOrderPosition++));

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }
    }
}