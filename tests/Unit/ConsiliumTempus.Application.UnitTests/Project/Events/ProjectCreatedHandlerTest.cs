using ConsiliumTempus.Application.Project.Events;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project.Events;

namespace ConsiliumTempus.Application.UnitTests.Project.Events;

public class ProjectCreatedHandlerTest
{
    #region Setup

    private readonly ProjectCreatedHandler _uut = new();

    #endregion

    [Fact]
    public async Task HandleProjectCreated_WhenSuccessful_ShouldAddSprintsStagesAndTasksToTheProject()
    {
        // Arrange
        var user = UserFactory.Create();
        var project = ProjectFactory.Create(user: user, sprintsCount: 0);
        var notification = new ProjectCreated(project, user);

        // Act
        await _uut.Handle(notification, default);

        // Assert
        project.Sprints.Should().HaveCount(1);
        project.Sprints[0].Id.Value.ToString().Should().NotBeNullOrWhiteSpace();
        project.Sprints[0].Project.Should().Be(project);
        project.Sprints[0].Name.Value.Should().Be(Constants.ProjectSprint.Name);
        project.Sprints[0].StartDate.Should().BeNull();
        project.Sprints[0].EndDate.Should().BeNull();
        project.Sprints[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        project.Sprints[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

        var count = 0;
        project.Sprints[0].Stages.Should().HaveCount(Constants.ProjectStage.Names.Length);
        project.Sprints[0].Stages
            .ToList()
            .ForEach(stage =>
            {
                stage.Id.Value.ToString().Should().NotBeNullOrWhiteSpace();
                stage.CustomOrderPosition.Value.Should().Be(count++);
                stage.Sprint.Should().Be(project.Sprints[0]);
            });
        project.Sprints[0].Stages
            .Zip(Constants.ProjectStage.Names)
            .ToList()
            .ForEach(x => x.First.Name.Value.Should().Be(x.Second));

        count = 0;
        project.Sprints[0].Stages[0].Tasks.Should().HaveCount(Constants.ProjectTask.Names.Length);
        project.Sprints[0].Stages[0].Tasks
            .Zip(Constants.ProjectTask.Names)
            .ToList()
            .ForEach(x => x.First.Name.Value.Should().Be(x.Second));
        project.Sprints[0].Stages[0].Tasks
            .ToList()
            .ForEach(task =>
            {
                task.Id.Value.ToString().Should().NotBeNullOrWhiteSpace();
                task.Description.Value.Should().BeEmpty();
                task.IsCompleted.Value.Should().BeFalse();
                task.CreatedBy.Should().Be(user);
                task.CustomOrderPosition.Value.Should().Be(count++);
                task.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
                task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
                task.Asignee.Should().BeNull();
                task.Reviewer.Should().BeNull();
                task.DueDate.Should().BeNull();
                task.EstimatedDuration.Should().BeNull();
                task.Stage.Should().Be(project.Sprints[0].Stages[0]);
                task.Comments.Should().BeEmpty();
            });
    }
}