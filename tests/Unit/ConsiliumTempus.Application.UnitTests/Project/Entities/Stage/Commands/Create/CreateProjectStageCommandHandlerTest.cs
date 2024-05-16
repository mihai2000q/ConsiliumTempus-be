using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Commands.Create;

public class CreateProjectStageCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly CreateProjectStageCommandHandler _uut;

    public CreateProjectStageCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new CreateProjectStageCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleCreateProjectStageCommand_WhenSprintIsNull_ShouldReturnSprintNotFoundError()
    {
        // Arrange
        var command = ProjectStageCommandFactory.CreateCreateProjectStageCommand();

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.ProjectSprintId));
        _projectSprintRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }

    [Theory]
    [ClassData(typeof(CreateProjectStageCommandHandlerData.GetCommands))]
    public async Task HandleCreateProjectStageCommand_WhenSuccessful_ShouldCreateProjectStageAndReturnSuccessfulResult(
        CreateProjectStageCommand command)
    {
        // Arrange
        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.ProjectSprintId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectStageResult());

        Utils.ProjectStage.AssertFromCreateCommand(command, sprint);
    }
}