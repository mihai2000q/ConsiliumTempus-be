using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.UpdateStage;

public class UpdateStageFromProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly UpdateStageFromProjectSprintCommandHandler _uut;

    public UpdateStageFromProjectSprintCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new UpdateStageFromProjectSprintCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateStageFromProjectSprintCommand_WhenSuccessful_ShouldUpdateAndReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        var expectedStageToUpdate = sprint.Stages[0];

        var command = ProjectSprintCommandFactory.CreateUpdateStageFromProjectSprintCommand(
            stageId: expectedStageToUpdate.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateStageFromProjectSprintResult());

        Utils.ProjectSprint.AssertUpdateStageCommand(expectedStageToUpdate, command);
    }

    [Fact]
    public async Task HandleUpdateStageFromProjectSprintCommand_WhenSprintIsNotFound_ShouldReturnSprintNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateUpdateStageFromProjectSprintCommand();

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }

    [Fact]
    public async Task HandleUpdateStageFromProjectSprintCommand_WhenStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateUpdateStageFromProjectSprintCommand();

        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}