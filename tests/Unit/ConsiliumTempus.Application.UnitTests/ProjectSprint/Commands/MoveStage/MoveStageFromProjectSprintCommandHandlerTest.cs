using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.MoveStage;

public class MoveStageFromProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly MoveStageFromProjectSprintCommandHandler _uut;

    public MoveStageFromProjectSprintCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new MoveStageFromProjectSprintCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(MoveStageFromProjectSprintCommandHandlerData.GetCommands))]
    public async Task HandleMoveStageFromProjectSprintCommand_WhenSuccessful_ShouldMoveAndReturnSuccessResponse(
        MoveStageFromProjectSprintCommand command,
        ProjectSprintAggregate sprint,
        ProjectStage stage,
        ProjectStage overStage)
    {
        // Arrange
        var expectedCustomOrderPosition = overStage.CustomOrderPosition.Value;

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _projectSprintRepository
            .Received(1)
            .MarkAsUpdated(stage);

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MoveStageFromProjectSprintResult());

        Utils.ProjectSprint.AssertFromMoveStageCommand(
            stage,
            command,
            expectedCustomOrderPosition);
    }

    [Fact]
    public async Task HandleMoveStageFromProjectSprintCommand_WhenOverStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(
            stageId: sprint.Stages[0].Id.Value);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectStage.NotFoundWithId(ProjectStageId.Create(command.OverStageId)));
    }

    [Fact]
    public async Task
        HandleMoveStageFromProjectSprintCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand();

        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectStage.NotFoundWithId(ProjectStageId.Create(command.StageId)));
    }

    [Fact]
    public async Task
        HandleMoveStageFromProjectSprintCommand_WhenProjectSprintIsNull_ShouldReturnProjectSprintNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand();

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}