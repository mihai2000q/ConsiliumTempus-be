using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Move;

public class MoveProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly MoveProjectTaskCommandHandler _uut;

    public MoveProjectTaskCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new MoveProjectTaskCommandHandler(_projectSprintRepository, _projectTaskRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(MoveProjectTaskCommandHandlerData.GetMovingToAnotherStageCommands))]
    public async Task HandleMoveProjectTaskCommand_WhenMovingToAnotherStage_ShouldMoveProjectTask(
        MoveProjectTaskCommand command,
        ProjectTaskAggregate task,
        List<ProjectStage> stages)
    {
        // Arrange
        _projectTaskRepository
            .GetWithSprint(Arg.Any<ProjectTaskId>(), Arg.Any<bool>())
            .Returns(task);

        _projectSprintRepository
            .GetStagesWithTasks(Arg.Any<ProjectSprintId>())
            .Returns(stages);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithSprint(Arg.Is<ProjectTaskId>(id => id.Value == command.Id), Arg.Is(false));
        await _projectSprintRepository
            .Received(1)
            .GetStagesWithTasks(Arg.Is(task.Stage.Sprint.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveProjectTaskResult());

        Utils.ProjectTask.AssertFromMoveCommandToAnotherStage(task, command, stages);
    }

    [Theory]
    [ClassData(typeof(MoveProjectTaskCommandHandlerData.GetMovingWithinStageCommands))]
    public async Task HandleMoveProjectTaskCommand_WhenMovingWithinStage_ShouldMoveProjectTask(
        MoveProjectTaskCommand command,
        ProjectTaskAggregate task,
        List<ProjectStage> stages,
        int expectedCustomOrderPosition)
    {
        // Arrange
        _projectTaskRepository
            .GetWithSprint(Arg.Any<ProjectTaskId>(), Arg.Any<bool>())
            .Returns(task);

        _projectSprintRepository
            .GetStagesWithTasks(Arg.Any<ProjectSprintId>())
            .Returns(stages);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithSprint(Arg.Is<ProjectTaskId>(id => id.Value == command.Id), Arg.Is(false));
        await _projectSprintRepository
            .Received(1)
            .GetStagesWithTasks(Arg.Is(task.Stage.Sprint.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveProjectTaskResult());

        Utils.ProjectTask.AssertFromMoveCommandWithinStage(task, command, expectedCustomOrderPosition);
    }

    [Theory]
    [ClassData(typeof(MoveProjectTaskCommandHandlerData.GetMovingOverTaskToAnotherStageCommands))]
    public async Task HandleMoveProjectTaskCommand_WhenMovingOverTaskToAnotherStage_ShouldMoveProjectTask(
        MoveProjectTaskCommand command,
        ProjectTaskAggregate task,
        List<ProjectStage> stages,
        int expectedCustomOrderPosition)
    {
        // Arrange
        _projectTaskRepository
            .GetWithSprint(Arg.Any<ProjectTaskId>(), Arg.Any<bool>())
            .Returns(task);

        _projectSprintRepository
            .GetStagesWithTasks(Arg.Any<ProjectSprintId>())
            .Returns(stages);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithSprint(Arg.Is<ProjectTaskId>(id => id.Value == command.Id), Arg.Is(false));
        await _projectSprintRepository
            .Received(1)
            .GetStagesWithTasks(Arg.Is(task.Stage.Sprint.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveProjectTaskResult());

        Utils.ProjectTask.AssertFromMoveCommandOverTaskToAnotherStage(
            task,
            command,
            stages,
            expectedCustomOrderPosition);
    }

    [Fact]
    public async Task HandleMoveProjectTaskCommand_WhenOverIsNull_ShouldReturnOverNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand();

        var stages = ProjectStageFactory.CreateListWithTasks();
        var task = stages[0].Tasks[0];

        _projectTaskRepository
            .GetWithSprint(Arg.Any<ProjectTaskId>(), Arg.Any<bool>())
            .Returns(task);

        // these stages do not have the overId and neither do the tasks
        _projectSprintRepository
            .GetStagesWithTasks(Arg.Any<ProjectSprintId>())
            .Returns(stages);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithSprint(Arg.Is<ProjectTaskId>(id => id.Value == command.Id), Arg.Is(false));
        await _projectSprintRepository
            .Received(1)
            .GetStagesWithTasks(Arg.Is(task.Stage.Sprint.Id));

        outcome.ValidateError(Errors.ProjectTask.OverNotFound);
    }

    [Fact]
    public async Task HandleMoveProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand();

        _projectTaskRepository
            .GetWithSprint(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithSprint(Arg.Is<ProjectTaskId>(id => id.Value == command.Id), Arg.Is(false));
        _projectSprintRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}