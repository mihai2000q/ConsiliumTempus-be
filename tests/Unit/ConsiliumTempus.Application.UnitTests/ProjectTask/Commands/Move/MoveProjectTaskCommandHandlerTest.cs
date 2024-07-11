using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.UnitTests.TestUtils;
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
    [ClassData(typeof(MoveProjectTaskCommandHandlerData.GetCommands))]
    public async Task HandleMoveProjectTaskCommand_WhenIsSuccessful_ShouldMoveProjectTask(
        MoveProjectTaskCommand command,
        ProjectTaskAggregate task,
        List<ProjectStage> stages)
    {
        // Arrange
        _projectTaskRepository
            .GetWithTasks(Arg.Any<ProjectTaskId>())
            .Returns(task);

        _projectSprintRepository
            .GetStages(Arg.Any<ProjectSprintId>())
            .Returns(stages);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        await _projectSprintRepository
            .Received(1)
            .GetStages(Arg.Is<ProjectSprintId>(id => id == task.Stage.Sprint.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveProjectTaskResult());

        Utils.ProjectTask.AssertFromMoveCommand(task, command, stages);
    }

    [Fact]
    public async Task HandleMoveProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand();

        _projectTaskRepository
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id))
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectTaskRepository
            .Received(1)
            .GetWithTasks(Arg.Is<ProjectTaskId>(id => id.Value == command.Id));
        _projectSprintRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}