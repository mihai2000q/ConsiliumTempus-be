using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
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

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class WorkspaceControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly WorkspaceController _uut;

    public WorkspaceControllerTest()
    {
        var mapper = Utils.GetMapper<WorkspaceMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new WorkspaceController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task GetWorkspace_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetWorkspaceRequest();

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

        var response = outcome.ToResponse<GetWorkspaceResponse>();
        Utils.Workspace.AssertGetResponse(response, result);
    }

    [Fact]
    public async Task GetWorkspace_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetWorkspaceRequest();

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
    public async Task GetCollectionWorkspace_WhenIsSuccessful_ShouldReturnCollectionOfWorkspaces()
    {
        // Arrange
        var result = new GetCollectionWorkspaceResult(WorkspaceFactory.CreateList());
        _mediator
            .Send(Arg.Any<GetCollectionWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollection(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionWorkspaceQuery>(query => Utils.Workspace.AssertGetCollectionQuery(query)));

        var response = outcome.ToResponse<GetCollectionWorkspaceResponse>();
        Utils.Workspace.AssertGetCollectionResponse(response, result);
    }

    [Fact]
    public async Task GetCollectionWorkspace_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollection(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionWorkspaceQuery>(query => Utils.Workspace.AssertGetCollectionQuery(query)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task CreateWorkspace_WhenIsSuccessful_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();

        var result = new CreateWorkspaceResult();
        _mediator
            .Send(Arg.Any<CreateWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateWorkspaceCommand>(
                command => Utils.Workspace.AssertCreateCommand(command, request)));

        var response = outcome.ToResponse<CreateWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task CreateWorkspace_WhenItFails_ShouldReturnUserPRoblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<CreateWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateWorkspaceCommand>(
                command => Utils.Workspace.AssertCreateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task DeleteWorkspace_WhenIsSuccessful_ShouldReturnSuccessResponse()
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

        var response = outcome.ToResponse<DeleteWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteWorkspace_WhenItFails_ShouldReturnProblem()
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
    public async Task UpdateWorkspace_WhenIsSuccessful_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest();

        var result = new UpdateWorkspaceResult();
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

        var response = outcome.ToResponse<UpdateWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateWorkspace_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest();

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