using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Commands.Delete;

public class DeleteProjectStageCommandHandlerTest
{
    #region Setup

    private readonly IProjectStageRepository _projectStageRepository;
    private readonly DeleteProjectStageCommandHandler _uut;
    
    public DeleteProjectStageCommandHandlerTest()
    {
        _projectStageRepository = Substitute.For<IProjectStageRepository>();
        _uut = new DeleteProjectStageCommandHandler(_projectStageRepository);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteProjectStageIsSuccessful_ShouldRemoveAndReturnSuccessfulResult()
    {
        // Arrange
        var projectStage = ProjectStageFactory.CreateWithStages();
        _projectStageRepository
            .GetWithStagesAndWorkspace(Arg.Any<ProjectStageId>())
            .Returns(projectStage);
        
        var command = new DeleteProjectStageCommand(Id: projectStage.Id.Value);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectStageRepository
            .Received(1)
            .GetWithStagesAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.Id));
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectStageResult());
        
        Utils.ProjectStage.AssertFromDeleteCommand(projectStage, command);
    }
    
    [Fact]
    public async Task WhenDeleteProjectStageFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteProjectStageCommand(Guid.NewGuid());

        _projectStageRepository
            .GetWithStagesAndWorkspace(Arg.Any<ProjectStageId>())
            .ReturnsNull();
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectStageRepository
            .Received(1)
            .GetWithStagesAndWorkspace(Arg.Is<ProjectStageId>(id => id.Value == command.Id));
        
        outcome.IsError.Should().BeTrue();
        outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}