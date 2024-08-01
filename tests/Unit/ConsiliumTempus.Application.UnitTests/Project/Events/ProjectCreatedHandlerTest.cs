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
        var owner = UserFactory.Create();
        var project = ProjectFactory.CreateWithSprints(owner: owner, sprintsCount: 0);
        var domainEvent = new ProjectCreated(project);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        project.Sprints.Should().HaveCount(1);
        var sprint = project.Sprints[0];
        sprint.Id.Value.ToString().Should().NotBeEmpty();
        sprint.Project.Should().Be(project);
        sprint.Name.Value.Should().Be(Constants.ProjectSprint.Name);
        sprint.StartDate.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
        sprint.EndDate.Should().BeNull();

        sprint.Audit.CreatedBy.Should().Be(owner);
        sprint.Audit.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        sprint.Audit.UpdatedBy.Should().Be(owner);
        sprint.Audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

        var count = 0;
        sprint.Stages.Should().HaveCount(Constants.ProjectStage.Names.Length);
        sprint.Stages
            .Should()
            .AllSatisfy(stage =>
            {
                stage.Id.Value.ToString().Should().NotBeEmpty();
                // ReSharper disable once AccessToModifiedClosure
                stage.CustomOrderPosition.Value.Should().Be(count++);
                stage.Sprint.Should().Be(project.Sprints[0]);
                
                stage.Audit.CreatedBy.Should().Be(owner);
                stage.Audit.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
                stage.Audit.UpdatedBy.Should().Be(owner);
                stage.Audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            });
        sprint.Stages
            .Zip(Constants.ProjectStage.Names)
            .Should()
            .AllSatisfy(x => x.First.Name.Value.Should().Be(x.Second));

        count = 0;
        var stage = sprint.Stages[0];
        stage.Tasks.Should().HaveCount(Constants.ProjectTask.Names.Length);
        stage.Tasks
            .Zip(Constants.ProjectTask.Names)
            .Should()
            .AllSatisfy(x => x.First.Name.Value.Should().Be(x.Second));
        stage.Tasks
            .Should()
            .AllSatisfy(task =>
            {
                task.Id.Value.ToString().Should().NotBeEmpty();
                task.Description.Value.Should().BeEmpty();
                task.IsCompleted.Value.Should().BeFalse();
                task.CreatedBy.Should().Be(owner);
                task.CustomOrderPosition.Value.Should().Be(count++);
                task.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
                task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
                task.Assignee.Should().BeNull();
                task.Reviewer.Should().BeNull();
                task.DueDate.Should().BeNull();
                task.EstimatedDuration.Should().BeNull();
                task.Stage.Should().Be(project.Sprints[0].Stages[0]);
                task.Comments.Should().BeEmpty();
            });
    }
}