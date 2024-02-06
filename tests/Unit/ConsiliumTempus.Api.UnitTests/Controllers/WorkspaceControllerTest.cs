using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<CreateWorkspaceResponse>();
        
        var response = ((OkObjectResult)outcome).Value as CreateWorkspaceResponse;
        Utils.Workspace.AssertCreateResponse(response!, result);
    }
    
    [Fact]
    public async Task WhenWorkspaceCreateFails_ShouldReturnInvalidTokenError()
    {
        // Arrange
        var request = new CreateWorkspaceRequest(
            "Workspace Name",
            "Workspace Description that is very long");

        const string token = "This-is-the-token";
        _httpContext.SetupGet(h => h.Request.Headers.Authorization)
            .Returns($"Bearer {token}");
        
        var error = Errors.Authentication.InvalidToken;
        _mediator.Setup(m => m.Send(It.IsAny<CreateWorkspaceCommand>(), default))
            .ReturnsAsync(error);
        
        // Act
        var outcome = await _uut.Create(request);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<CreateWorkspaceCommand>(
                    command => Utils.Workspace.AssertCreateCommand(command, request, token)),
                default), 
            Times.Once());

        outcome.ValidateError(StatusCodes.Status401Unauthorized, "Invalid Token");
    }
}