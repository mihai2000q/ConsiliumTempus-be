using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;

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
    public async Task GetOverview_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetOverviewProjectRequest();

        var projectOverview = new GetOverviewProjectResult(Description.Create("this is a description"));
        _mediator
            .Send(Arg.Any<GetOverviewProjectQuery>())
            .Returns(projectOverview);

        // Act
        var outcome = await _uut.GetOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetOverviewProjectQuery>(q =>
                Utils.Project.AssertGetOverviewProjectQuery(q, request)));

        var response = outcome.ToResponse<GetOverviewProjectResponse>();
        Utils.Project.AssertGetOverviewProjectResponse(response, projectOverview);
    }

    [Fact]
    public async Task GetOverview_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetOverviewProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<GetOverviewProjectQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetOverviewProjectQuery>(q =>
                Utils.Project.AssertGetOverviewProjectQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollection_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest();

        var result = ProjectResultFactory.CreateGetCollectionProjectResult();
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
    public async Task Create_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest();

        var result = ProjectResultFactory.CreateCreateProjectResult();
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
    public async Task Create_WhenItFails_ShouldReturnProblem()
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
    public async Task Update_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateProjectRequest();

        var result = ProjectResultFactory.CreateUpdateProjectResult();
        _mediator
            .Send(Arg.Any<UpdateProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectCommand>(command => Utils.Project.AssertUpdateCommand(command, request)));

        var response = outcome.ToResponse<UpdateProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Update_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateProjectRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectCommand>(command => Utils.Project.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Delete_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateDeleteProjectRequest();

        var result = ProjectResultFactory.CreateDeleteProjectResult();
        _mediator
            .Send(Arg.Any<DeleteProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectCommand>(command => Utils.Project.AssertDeleteCommand(command, request)));

        var response = outcome.ToResponse<DeleteProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Delete_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateDeleteProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<DeleteProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectCommand>(command => Utils.Project.AssertDeleteCommand(command, request)));

        outcome.ValidateError(error);
    }
}