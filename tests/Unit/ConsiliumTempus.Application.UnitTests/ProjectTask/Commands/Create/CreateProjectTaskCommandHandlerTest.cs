using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Create;

public class CreateProjectTaskCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly CreateProjectTaskCommandHandler _uut;

    public CreateProjectTaskCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new CreateProjectTaskCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(CreateProjectTaskCommandHandlerData.GetCommands))]
    public async Task HandleCreateProjectTaskCommand_WhenIsSuccessful_ShouldCreateAndSaveProjectTaskOnProjectStage(
        CreateProjectTaskCommand command)
    {
        // Arrange
        var stage = ProjectStageFactory.CreateWithTasks();
        _projectSprintRepository
            .GetStageWithTasksAndWorkspace(Arg.Any<ProjectStageId>())
            .Returns(stage);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetStageWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.ProjectStageId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectTaskResult());

        Utils.ProjectTask.AssertFromCreateCommand(command, stage);
    }

    [Fact]
    public async Task HandleCreateProjectTaskCommand_WhenProjectStageIsNull_ShouldReturnProjectStageNotFoundError()
    {
        // Arrange
        var command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand();

        _projectSprintRepository
            .GetStageWithTasksAndWorkspace(Arg.Any<ProjectStageId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetStageWithTasksAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.ProjectStageId));

        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}