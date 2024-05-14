using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static bool AssertFromCreateCommand(
            ProjectTaskAggregate project,
            CreateProjectTaskCommand command,
            Domain.Project.Entities.ProjectStage stage,
            UserAggregate user)
        {
            project.Id.Value.Should().NotBeEmpty();
            project.Name.Value.Should().Be(command.Name);
            project.Description.Value.Should().Be(string.Empty);
            project.CustomOrderPosition.Value.Should().Be(stage.Tasks.Count);
            project.IsCompleted.Value.Should().Be(false);
            project.CreatedBy.Should().Be(user);
            project.Asignee.Should().BeNull();
            project.Reviewer.Should().BeNull();
            project.DueDate.Should().BeNull();
            project.EstimatedDuration.Should().BeNull();
            project.Stage.Should().Be(stage);
            project.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            project.Comments.Should().BeEmpty();

            return true;
        }
    }
}