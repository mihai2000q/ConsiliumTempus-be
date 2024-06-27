using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.Workspace.Queries.GetOverview;
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
    public async Task Get_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateGetWorkspaceResult();
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
    public async Task Get_WhenItFails_ShouldReturnProblem()
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
    public async Task GetOverview_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetOverviewWorkspaceRequest();

        var workspace = WorkspaceFactory.Create();
        _mediator
            .Send(Arg.Any<GetOverviewWorkspaceQuery>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.GetOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetOverviewWorkspaceQuery>(query => 
                Utils.Workspace.AssertGetOverviewQuery(query, request)));

        var response = outcome.ToResponse<GetOverviewWorkspaceResponse>();
        Utils.Workspace.AssertGetOverviewResponse(response, workspace);
    }

    [Fact]
    public async Task GetOverview_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetOverviewWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<GetOverviewWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetOverviewWorkspaceQuery>(query => 
                Utils.Workspace.AssertGetOverviewQuery(query, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task GetCollaborators_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateGetCollaboratorsFromWorkspaceResult();
        _mediator
            .Send(Arg.Any<GetCollaboratorsFromWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollaborators(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollaboratorsFromWorkspaceQuery>(query => 
                Utils.Workspace.AssertGetCollaboratorsQuery(query, request)));

        var response = outcome.ToResponse<GetCollaboratorsFromWorkspaceResponse>();
        Utils.Workspace.AssertGetCollaboratorsResponse(response, result);
    }

    [Fact]
    public async Task GetCollaborators_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<GetCollaboratorsFromWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollaborators(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollaboratorsFromWorkspaceQuery>(query => 
                Utils.Workspace.AssertGetCollaboratorsQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollection_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateGetCollectionWorkspaceResult();
        _mediator
            .Send(Arg.Any<GetCollectionWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetCollectionQuery(query, request)));

        var response = outcome.ToResponse<GetCollectionWorkspaceResponse>();
        Utils.Workspace.AssertGetCollectionResponse(response, result);
    }

    [Fact]
    public async Task GetCollection_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetCollectionQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Create_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateCreateWorkspaceResult();
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
    public async Task Create_WhenItFails_ShouldReturnProblem()
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
    public async Task Update_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateUpdateWorkspaceResult();
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
    public async Task Update_WhenItFails_ShouldReturnProblem()
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

    [Fact]
    public async Task Delete_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateDeleteWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateDeleteWorkspaceResult();
        _mediator
            .Send(Arg.Any<DeleteWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteWorkspaceCommand>(command =>
                Utils.Workspace.AssertDeleteCommand(command, request)));

        var response = outcome.ToResponse<DeleteWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Delete_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateDeleteWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<DeleteWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteWorkspaceCommand>(command =>
                Utils.Workspace.AssertDeleteCommand(command, request)));

        outcome.ValidateError(error);
    }
}