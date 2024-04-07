using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;
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
    public async Task GetCollectionForWorkspace_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectForWorkspaceRequest();
        
        var result = new GetCollectionProjectForWorkspaceResult(ProjectFactory.CreateList());
        _mediator
            .Send(Arg.Any<GetCollectionProjectForWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollectionForWorkspace(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectForWorkspaceQuery>(q => 
                Utils.Project.AssertGetCollectionProjectForWorkspaceQuery(q, request)));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<GetCollectionProjectForWorkspaceResponse>();

        var response = ((OkObjectResult)outcome).Value as GetCollectionProjectForWorkspaceResponse;
        response!.Projects.Zip(result.Projects)
            .Should().AllSatisfy(p => Utils.Project.AssertProjectResponse(p.First, p.Second));
    }
    
    [Fact]
    public async Task GetCollectionForWorkspace_WhenFails_ShouldWorkspaceNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectForWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionProjectForWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollectionForWorkspace(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectForWorkspaceQuery>(q => 
                Utils.Project.AssertGetCollectionProjectForWorkspaceQuery(q, request)));
        
        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task GetCollectionForUser_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var result = new GetCollectionProjectForUserResult(ProjectFactory.CreateList());
        _mediator
            .Send(Arg.Any<GetCollectionProjectForUserQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollectionForUser(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(new GetCollectionProjectForUserQuery());

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<GetCollectionProjectForUserResponse>();

        var response = ((OkObjectResult)outcome).Value as GetCollectionProjectForUserResponse;
        response!.Projects.Zip(result.Projects)
            .Should().AllSatisfy(p => Utils.Project.AssertProjectResponse(p.First, p.Second));
    }
    
    [Fact]
    public async Task GetCollectionForUser_WhenFails_ShouldUserNotFoundError()
    {
        // Arrange
        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionProjectForUserQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollectionForUser(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(new GetCollectionProjectForUserQuery());
        
        outcome.ValidateError(error);
    }

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