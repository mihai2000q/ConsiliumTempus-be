using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class WorkspaceControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly HttpContext _httpContext;
    private readonly WorkspaceController _uut;

    public WorkspaceControllerTest()
    {
        var mapper = Utils.GetMapper<WorkspaceMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new WorkspaceController(mapper, _mediator);

        _httpContext = Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task GetWorkspace_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var request = new GetWorkspaceRequest();

        var result = WorkspaceFactory.Create();
        _mediator
            .Send(Arg.Any<GetWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetWorkspaceQuery>(query => Utils.Workspace.AssertGetQuery(query, request)));
        
        Utils.Workspace.AssertDto(outcome, result);
    }

    [Fact]
    public async Task GetWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = new GetWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<GetWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetWorkspaceQuery>(query => Utils.Workspace.AssertGetQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollectionWorkspace_ShouldReturnCollectionOfWorkspaces()
    {
        // Arrange
        const string token = "This-is-a-token";
        _httpContext.Request.Headers.Authorization
            .Returns(new StringValues($"Bearer {token}"));

        var result = WorkspaceFactory.CreateList();
        _mediator
            .Send(Arg.Any<GetCollectionWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollection(default);

        // Assert
        _httpContext
            .Received(1);
        
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionWorkspaceQuery>(
                query => Utils.Workspace.AssertGetCollectionQuery(query, token)));
        
        Utils.Workspace.AssertDtos(outcome, result);
    }

    [Fact]
    public async Task WhenWorkspaceCreateIsSuccessful_ShouldReturnNewWorkspace()
    {
        // Arrange
        var request = new CreateWorkspaceRequest(
            "Workspace Name",
            "Workspace Description that is very long");

        const string token = "This-is-the-token";
        _httpContext.Request.Headers.Authorization
            .Returns(new StringValues($"Bearer {token}"));

        var result = new CreateWorkspaceResult(WorkspaceFactory.Create(request.Name, request.Description));
        _mediator
            .Send(Arg.Any<CreateWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        _httpContext.Received(1);
            
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateWorkspaceCommand>(
                command => Utils.Workspace.AssertCreateCommand(command, request, token)));

        Utils.Workspace.AssertDto(outcome, result.Workspace);
    }

    [Fact]
    public async Task DeleteWorkspace_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var id = Guid.NewGuid();

        var result = new DeleteWorkspaceResult();
        _mediator
            .Send(Arg.Any<DeleteWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteWorkspaceCommand>(command => command.Id == id));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<DeleteWorkspaceResponse>();

        var response = ((OkObjectResult)outcome).Value as DeleteWorkspaceResponse;
        response!.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<DeleteWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteWorkspaceCommand>(command => command.Id == id));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task UpdateWorkspace_WhenIsSuccessful_ShouldReturnNewWorkspace()
    {
        // Arrange
        var request = new UpdateWorkspaceRequest(
            Guid.NewGuid(),
            "New Name",
            "New Description");

        var result = new UpdateWorkspaceResult(WorkspaceFactory.Create());
        _mediator
            .Send(Arg.Any<UpdateWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateWorkspaceCommand>(
                command => Utils.Workspace.AssertUpdateCommand(command, request)));

        Utils.Workspace.AssertDto(outcome, result.Workspace);
    }

    [Fact]
    public async Task UpdateWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = new UpdateWorkspaceRequest(
            Guid.NewGuid(),
            "New Name",
            "New Description");

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateWorkspaceCommand>(
                command => Utils.Workspace.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }
}