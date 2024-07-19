using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Delete;

public class DeleteProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly DeleteProjectTaskCommandHandler _uut;

    public DeleteProjectTaskCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new DeleteProjectTaskCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleDeleteProjectTaskCommand_WhenIsSuccessful_ShouldDeleteProjectTask()
    {
        // Arrange
        var stage = ProjectStageFactory.CreateWithTasks();
        var task = stage.Tasks[0];
        _projectSprintRepository
            .GetStageWithTasksAndWorkspace(Arg.Any<ProjectStageId>())
            .Returns(stage);
        
        var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand(
            task.Id.Value,
            stage.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetStageWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.StageId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectTaskResult());

        Utils.ProjectTask.AssertFromDeleteCommand(task, command);
    }

    [Fact]
    public async Task HandleDeleteProjectTaskCommand_WhenProjectTaskIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand();

        var stage = ProjectStageFactory.CreateWithTasks();
        _projectSprintRepository
            .GetStageWithTasksAndWorkspace(Arg.Any<ProjectStageId>())
            .Returns(stage);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetStageWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.StageId));

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }

    [Fact]
    public async Task HandleDeleteProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand();

        _projectSprintRepository
            .GetStageWithTasksAndWorkspace(Arg.Any<ProjectStageId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetStageWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.StageId));

        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}