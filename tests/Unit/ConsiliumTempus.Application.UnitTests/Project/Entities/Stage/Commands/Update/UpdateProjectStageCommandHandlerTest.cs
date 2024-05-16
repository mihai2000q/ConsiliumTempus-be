using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Commands.Update;

public class UpdateProjectStageCommandHandlerTest
{
    #region Setup

    private readonly IProjectStageRepository _projectStageRepository;
    private readonly UpdateProjectStageCommandHandler _uut;

    public UpdateProjectStageCommandHandlerTest()
    {
        _projectStageRepository = Substitute.For<IProjectStageRepository>();
        _uut = new UpdateProjectStageCommandHandler(_projectStageRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateProjectStageCommand_WhenIsSuccessful_ShouldUpdateAndReturnSuccessResult()
    {
        // Arrange
        var stage = ProjectStageFactory.Create();
        _projectStageRepository
            .GetWithWorkspace(Arg.Any<ProjectStageId>())
            .Returns(stage);
        
        var command = ProjectStageCommandFactory.CreateUpdateProjectStageCommand(id: stage.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectStageRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateProjectStageResult());
        
        Utils.ProjectStage.AssertFromUpdateCommand(stage, command);
    }

    [Fact]
    public async Task HandleUpdateProjectStageCommand_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectStageCommandFactory.CreateUpdateProjectStageCommand();

        _projectStageRepository
            .GetWithWorkspace(Arg.Any<ProjectStageId>())
            .ReturnsNull();
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectStageRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}