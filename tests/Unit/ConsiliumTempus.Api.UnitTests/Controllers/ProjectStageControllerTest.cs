using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class ProjectStageControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly ProjectStageController _uut;

    public ProjectStageControllerTest()
    {
        var mapper = Utils.GetMapper<ProjectStageMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new ProjectStageController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task CreateProjectStage_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateCreateProjectStageRequest();

        var result = new CreateProjectStageResult();
        _mediator
            .Send(Arg.Any<CreateProjectStageCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectStageCommand>(
                command => Utils.ProjectStage.AssertCreateCommand(command, request)));

        var response = outcome.ToResponse<CreateProjectStageResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task CreateProjectStage_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateCreateProjectStageRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<CreateProjectStageCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectStageCommand>(
                command => Utils.ProjectStage.AssertCreateCommand(command, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task UpdateProjectStage_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateUpdateProjectStageRequest();

        var result = new UpdateProjectStageResult();
        _mediator
            .Send(Arg.Any<UpdateProjectStageCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectStageCommand>(
                command => Utils.ProjectStage.AssertUpdateCommand(command, request)));

        var response = outcome.ToResponse<UpdateProjectStageResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateProjectStage_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateUpdateProjectStageRequest();

        var error = Errors.ProjectStage.NotFound;
        _mediator
            .Send(Arg.Any<UpdateProjectStageCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectStageCommand>(
                command => Utils.ProjectStage.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task DeleteProjectStage_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();

        var result = new DeleteProjectStageResult();
        _mediator
            .Send(Arg.Any<DeleteProjectStageCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectStageCommand>(command =>
                Utils.ProjectStage.AssertDeleteCommand(command, id)));

        var response = outcome.ToResponse<DeleteProjectStageResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteProjectStage_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var id = Guid.NewGuid();

        var error = Errors.ProjectStage.NotFound;
        _mediator
            .Send(Arg.Any<DeleteProjectStageCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectStageCommand>(command =>
                Utils.ProjectStage.AssertDeleteCommand(command, id)));

        outcome.ValidateError(error);
    }
}