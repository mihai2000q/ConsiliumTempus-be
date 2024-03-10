using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class ProjectControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly ProjectController _uut;

    public ProjectControllerTest()
    {
        var mapper = Utils.GetMapper<ProjectMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new ProjectController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task CreateProject_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest();

        var result = new CreateProjectResult();
        _mediator
            .Send(Arg.Any<CreateProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectCommand>(command => Utils.Project.AssertCreateCommand(command, request)));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<CreateProjectResponse>();

        var response = ((OkObjectResult)outcome).Value as CreateProjectResponse;
        response!.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task CreateProject_WhenWorkspaceIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<CreateProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectCommand>(command => Utils.Project.AssertCreateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task DeleteProject_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();

        var result = new DeleteProjectResult();
        _mediator
            .Send(Arg.Any<DeleteProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectCommand>(command => Utils.Project.AssertDeleteCommand(command, id)));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<DeleteProjectResponse>();

        var response = ((OkObjectResult)outcome).Value as DeleteProjectResponse;
        response!.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteProject_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<DeleteProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectCommand>(command => Utils.Project.AssertDeleteCommand(command, id)));

        outcome.ValidateError(error);
    }
}