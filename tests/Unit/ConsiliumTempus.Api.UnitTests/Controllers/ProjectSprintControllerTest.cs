using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class ProjectSprintControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly ProjectSprintController _uut;

    public ProjectSprintControllerTest()
    {
        var mapper = Utils.GetMapper<ProjectSprintMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new ProjectSprintController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion
    
    [Fact]
    public async Task CreateProject_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = new CreateProjectSprintRequest(
            Guid.NewGuid(),
            "Project Sprint Name",
            null,
            null);

        var result = new CreateProjectSprintResult();
        _mediator
            .Send(Arg.Any<CreateProjectSprintCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectSprintCommand>(
                command => Utils.ProjectSprint.AssertCreateCommand(command, request)));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<CreateProjectSprintResponse>();

        var response = ((OkObjectResult)outcome).Value as CreateProjectSprintResponse;
        response!.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task CreateProject_WhenWorkspaceIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = new CreateProjectSprintRequest(
            Guid.NewGuid(),
            "Project Sprint Name",
            null,
            null);

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<CreateProjectSprintCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectSprintCommand>(
                command => Utils.ProjectSprint.AssertCreateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task DeleteProject_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();

        var result = new DeleteProjectSprintResult();
        _mediator
            .Send(Arg.Any<DeleteProjectSprintCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectSprintCommand>(command => 
                Utils.ProjectSprint.AssertDeleteCommand(command, id)));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<DeleteProjectSprintResponse>();

        var response = ((OkObjectResult)outcome).Value as DeleteProjectSprintResponse;
        response!.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteProject_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        var error = Errors.ProjectSprint.NotFound;
        _mediator
            .Send(Arg.Any<DeleteProjectSprintCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectSprintCommand>(command => 
                Utils.ProjectSprint.AssertDeleteCommand(command, id)));

        outcome.ValidateError(error);
    }
}