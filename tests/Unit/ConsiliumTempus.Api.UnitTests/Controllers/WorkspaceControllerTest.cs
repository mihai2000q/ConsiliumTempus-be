using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class WorkspaceControllerTest
{
    #region Setup

    private readonly Mock<ISender> _mediator;
    private readonly Mock<HttpContext> _httpContext;
    private readonly WorkspaceController _uut;

    public WorkspaceControllerTest()
    {
        var mapper = Utils.GetMapper<WorkspaceMappingConfig>();
        
        _mediator = new Mock<ISender>();
        _uut = new WorkspaceController(mapper, _mediator.Object);

        _httpContext = Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task GetWorkspace_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var request = new GetWorkspaceRequest();

        var result = new GetWorkspaceResult(Mock.Mock.Workspace.CreateMock());
        _mediator.Setup(m => m.Send(It.IsAny<GetWorkspaceQuery>(), default))
            .ReturnsAsync(result);
        
        // Act
        var outcome = await _uut.Get(request);

        // Assert
        Utils.Workspace.AssertDto(outcome, result.Workspace);
    }
    
    [Fact]
    public async Task GetWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = new GetWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator.Setup(m => m.Send(It.IsAny<GetWorkspaceQuery>(), default))
            .ReturnsAsync(error);
        
        // Act
        var outcome = await _uut.Get(request);

        // Assert
        outcome.ValidateError(StatusCodes.Status404NotFound, "Workspace could not be found");
    }
    
    [Fact]
    public async Task WhenWorkspaceCreateIsSuccessful_ShouldReturnNewWorkspace()
    {
        // Arrange
        var request = new CreateWorkspaceRequest(
            "Workspace Name",
            "Workspace Description that is very long");

        const string token = "This-is-the-token";
        _httpContext.SetupGet(h => h.Request.Headers.Authorization)
            .Returns($"Bearer {token}");
        
        var result = new CreateWorkspaceResult(Mock.Mock.Workspace.CreateMock(request.Name, request.Description));
        _mediator.Setup(m => m.Send(It.IsAny<CreateWorkspaceCommand>(), default))
            .ReturnsAsync(result);
        
        // Act
        var outcome = await _uut.Create(request);

        // Assert
        _mediator.Verify(m => m.Send(It.Is<CreateWorkspaceCommand>(
                command => Utils.Workspace.AssertCreateCommand(command, request, token)),
                default), 
            Times.Once());
        
        Utils.Workspace.AssertDto(outcome, result.Workspace);
    }
}