using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class ProjectControllerTest
{
    #region Setup

    private readonly Mock<ISender> _mediator;
    private readonly Mock<HttpContext> _httpContext;
    private readonly ProjectController _uut;
    
    public ProjectControllerTest()
    {
        var mapper = Utils.GetMapper<ProjectMappingConfig>();

        _mediator = new Mock<ISender>();
        _uut = new ProjectController(mapper, _mediator.Object);

        _httpContext = Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task CreateProject_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Guid.NewGuid(), 
            "Project Name",
            "This is the project description",
            true);

        const string token = "My-Token";
        _httpContext.SetupGet(h => h.Request.Headers.Authorization)
            .Returns($"Bearer {token}");

        var result = new CreateProjectResult();
        _mediator.Setup(m => m.Send(It.IsAny<CreateProjectCommand>(), default))
            .ReturnsAsync(result);
        
        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        _mediator.Verify(m =>
                m.Send(It.Is<CreateProjectCommand>(
                        c => Utils.Project.AssertCreateCommand(c, request, token)),
                    default),
            Times.Once);
        
        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<CreateProjectResponse>();

        var response = ((OkObjectResult)outcome).Value as CreateProjectResponse;
        response!.Message.Should().Be(result.Message);
    }
    
    [Fact]
    public async Task CreateProject_WhenWorkspaceIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Guid.NewGuid(), 
            "Project Name",
            "This is the project description",
            true);

        const string token = "My-Token";
        _httpContext.SetupGet(h => h.Request.Headers.Authorization)
            .Returns($"Bearer {token}");

        var error = Errors.Workspace.NotFound;
        _mediator.Setup(m => m.Send(It.IsAny<CreateProjectCommand>(), default))
            .ReturnsAsync(error);
        
        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        _mediator.Verify(m =>
                m.Send(It.Is<CreateProjectCommand>(
                        c => Utils.Project.AssertCreateCommand(c, request, token)),
                    default),
            Times.Once);
        
        outcome.ValidateError(StatusCodes.Status404NotFound, error.Description);
    }

    [Fact]
    public async Task DeleteProject_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        var result = new DeleteProjectResult();
        _mediator.Setup(m => m.Send(It.IsAny<DeleteProjectCommand>(), default))
            .ReturnsAsync(result);
        
        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        _mediator.Verify(m =>
                m.Send(It.Is<DeleteProjectCommand>(
                        c => Utils.Project.AssertDeleteCommand(c, id)),
                    default),
            Times.Once);
        
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
        _mediator.Setup(m => m.Send(It.IsAny<DeleteProjectCommand>(), default))
            .ReturnsAsync(error);
        
        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        _mediator.Verify(m =>
                m.Send(It.Is<DeleteProjectCommand>(
                        c => Utils.Project.AssertDeleteCommand(c, id)),
                    default),
            Times.Once);
        
        outcome.ValidateError(StatusCodes.Status404NotFound, error.Description);
    }
}