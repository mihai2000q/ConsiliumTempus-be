using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Move;

public class MoveProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly MoveProjectTaskCommandHandler _uut;

    public MoveProjectTaskCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new MoveProjectTaskCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(MoveProjectTaskCommandHandlerData.GetMovingToAnotherStageCommands))]
    public async Task HandleMoveProjectTaskCommand_WhenMovingToAnotherStage_ShouldMoveProjectTask(
        MoveProjectTaskCommand command,
        ProjectSprintAggregate sprint,
        ProjectTaskAggregate task)
    {
        // Arrange
        _projectSprintRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.SprintId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveProjectTaskResult());

        Utils.ProjectTask.AssertFromMoveCommandToAnotherStage(task, command);
    }

    [Theory]
    [ClassData(typeof(MoveProjectTaskCommandHandlerData.GetMovingWithinStageCommands))]
    public async Task HandleMoveProjectTaskCommand_WhenMovingWithinStage_ShouldMoveProjectTask(
        MoveProjectTaskCommand command,
        ProjectSprintAggregate sprint,
        ProjectTaskAggregate task,
        int expectedCustomOrderPosition)
    {
        // Arrange
        _projectSprintRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.SprintId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveProjectTaskResult());

        Utils.ProjectTask.AssertFromMoveCommandWithinStage(task, command, expectedCustomOrderPosition);
    }

    [Theory]
    [ClassData(typeof(MoveProjectTaskCommandHandlerData.GetMovingOverTaskToAnotherStageCommands))]
    public async Task HandleMoveProjectTaskCommand_WhenMovingOverTaskToAnotherStage_ShouldMoveProjectTask(
        MoveProjectTaskCommand command,
        ProjectSprintAggregate sprint,
        ProjectTaskAggregate task,
        int expectedCustomOrderPosition)
    {
        // Arrange
        _projectSprintRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.SprintId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveProjectTaskResult());

        Utils.ProjectTask.AssertFromMoveCommandOverTaskToAnotherStage(
            task,
            command,
            expectedCustomOrderPosition);
    }

    [Fact]
    public async Task HandleMoveProjectTaskCommand_WhenOverIsNull_ShouldReturnOverNotFoundError()
    {
        // Arrange
        var sprint = ProjectSprintFactory.Create(stagesCount: 0);
        var stages = ProjectStageFactory.CreateListWithTasks();
        sprint.AddStages(stages);
        var task = stages[0].Tasks[0];
        _projectSprintRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
            sprint.Id.Value,
            task.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.SprintId));

        outcome.ValidateError(Errors.ProjectTask.OverNotFound);
    }

    [Fact]
    public async Task HandleMoveProjectTaskCommand_WhenProjectTaskIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand();

        var sprint = ProjectSprintFactory.Create(stagesCount: 0);
        var stages = ProjectStageFactory.CreateListWithTasks();
        sprint.AddStages(stages);
        _projectSprintRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.SprintId));

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }

    [Fact]
    public async Task HandleMoveProjectTaskCommand_WhenProjectSprintIsNull_ShouldReturnProjectSprintNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand();

        _projectSprintRepository
            .GetWithTasksAndWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectSprintRepository
            .Received(1)
            .GetWithTasksAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.SprintId));

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}