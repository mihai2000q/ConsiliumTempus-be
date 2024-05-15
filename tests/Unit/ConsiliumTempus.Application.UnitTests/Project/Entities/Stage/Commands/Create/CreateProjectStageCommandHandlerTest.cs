using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Commands.Create;

public class CreateProjectStageCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly IProjectStageRepository _projectStageRepository;
    private readonly CreateProjectStageCommandHandler _uut;

    public CreateProjectStageCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _projectStageRepository = Substitute.For<IProjectStageRepository>();
        _uut = new CreateProjectStageCommandHandler(_projectSprintRepository, _projectStageRepository);
    }

    #endregion

    [Fact]
    public async Task HandleCreateProjectStageCommand_WhenSprintIsNull_ShouldReturnSprintNotFoundError()
    {
        // Arrange
        var command = ProjectStageCommandFactory.CreateCreateProjectStageCommand();

        _projectSprintRepository
            .GetWithStagesAndWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithStagesAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.ProjectSprintId));
        _projectSprintRepository.DidNotReceive();

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }

    [Fact]
    public async Task HandleCreateProjectStageCommand_WhenSuccessful_ShouldCreateProjectStageAndReturnSuccessfulResult()
    {
        // Arrange
        var command = ProjectStageCommandFactory.CreateCreateProjectStageCommand();

        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithStagesAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithStagesAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.ProjectSprintId));
        await _projectStageRepository
            .Received(1)
            .Add(Arg.Is<ProjectStage>(stage => Utils.ProjectStage.AssertFromCreateCommand(stage, command, sprint)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectStageResult());
    }
}