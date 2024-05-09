using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;

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
    public async Task Get_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetProjectRequest();

        var project = ProjectFactory.Create();
        _mediator
            .Send(Arg.Any<GetProjectQuery>())
            .Returns(project);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetProjectQuery>(q =>
                Utils.Project.AssertGetProjectQuery(q, request)));

        var response = outcome.ToResponse<GetProjectResponse>();
        Utils.Project.AssertGetProjectResponse(response, project);
    }

    [Fact]
    public async Task Get_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<GetProjectQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetProjectQuery>(q =>
                Utils.Project.AssertGetProjectQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollection_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest();

        var result = new GetCollectionProjectResult(
            ProjectFactory.CreateList(),
            25,
            null);
        _mediator
            .Send(Arg.Any<GetCollectionProjectQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectQuery>(q =>
                Utils.Project.AssertGetCollectionProjectQuery(q, request)));

        var response = outcome.ToResponse<GetCollectionProjectResponse>();
        Utils.Project.AssertGetCollectionResponse(response, result);
    }

    [Fact]
    public async Task GetCollection_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionProjectQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectQuery>(q =>
                Utils.Project.AssertGetCollectionProjectQuery(q, request)));

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
            .Send(Arg.Is<GetCollectionProjectForUserQuery>(q =>
                Utils.Project.AssertGetCollectionProjectForUserQuery(q)));

        var response = outcome.ToResponse<GetCollectionProjectForUserResponse>();
        Utils.Project.AssertGetCollectionForUserResponse(response, result);
    }

    [Fact]
    public async Task GetCollectionForUser_WhenItFails_ShouldReturnProblem()
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
            .Send(Arg.Is<GetCollectionProjectForUserQuery>(q =>
                Utils.Project.AssertGetCollectionProjectForUserQuery(q)));

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

        var response = outcome.ToResponse<CreateProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task CreateProject_WhenItFails_ShouldReturnProblem()
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

        var response = outcome.ToResponse<DeleteProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteProject_WhenItFails_ShouldReturnProblem()
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