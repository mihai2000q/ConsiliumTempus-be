using ConsiliumTempus.Application.Project.Events;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project.Events;

namespace ConsiliumTempus.Application.UnitTests.Project.Events;

public class ProjectCreatedHandlerTest
{
    #region Setup

    private readonly ProjectCreatedHandler _uut = new();

    #endregion

    [Fact]
    public async Task WhenProjectCreatedIsSuccessful_ShouldAddMoreDataToTheProject()
    {
        // Arrange
        var user = Mock.Mock.User.CreateMock();
        var workspace = Mock.Mock.Workspace.CreateMock();
        var project = Mock.Mock.Project.CreateMock(workspace, user);
        var notification = new ProjectCreated(project, user);
        
        // Act
        await _uut.Handle(notification, default);

        // Assert
        project.Sprints.Should().HaveCount(1);
        project.Sprints[0].Id.Value.ToString().Should().NotBeNullOrWhiteSpace();
        project.Sprints[0].Project.Should().Be(project);
        project.Sprints[0].Name.Should().Be(Constants.ProjectSprint.Name);
        project.Sprints[0].StartDate.Should().BeNull();
        project.Sprints[0].EndDate.Should().BeNull();
        project.Sprints[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        project.Sprints[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        
        project.Sprints[0].Sections.Should().HaveCount(Constants.ProjectSection.Names.Length);
        project.Sprints[0].Sections
            .ToList()
            .ForEach(section =>
            {
                section.Id.Value.ToString().Should().NotBeNullOrWhiteSpace();
                section.Sprint.Should().Be(project.Sprints[0]);
            });
        project.Sprints[0].Sections
            .Zip(Constants.ProjectSection.Names)
            .ToList()
            .ForEach(x => x.First.Name.Should().Be(x.Second));

        project.Sprints[0].Sections[0].Tasks.Should().HaveCount(Constants.ProjectTask.Names.Length);
        project.Sprints[0].Sections[0].Tasks
            .Zip(Constants.ProjectTask.Names)
            .ToList()
            .ForEach(x => x.First.Name.Should().Be(x.Second));
        project.Sprints[0].Sections[0].Tasks
            .ToList()
            .ForEach(task =>
            {
                task.Id.Value.ToString().Should().NotBeNullOrWhiteSpace();
                task.Description.Should().BeEmpty();
                task.IsCompleted.Should().BeFalse();
                task.CreatedBy.Should().Be(user);
                task.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
                task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
                task.Asignee.Should().BeNull();
                task.Reviewer.Should().BeNull();
                task.DueDate.Should().BeNull();
                task.EstimatedDuration.Should().BeNull();
                task.Section.Should().Be(project.Sprints[0].Sections[0]);
                task.Comments.Should().BeEmpty();
            });
    }
}