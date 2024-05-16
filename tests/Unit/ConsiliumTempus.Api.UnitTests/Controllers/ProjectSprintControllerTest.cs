using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Update;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Domain.Common.Errors;

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
    public async Task GetProjectSprint_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetProjectSprintRequest();

        var sprint = ProjectSprintFactory.Create();
        _mediator
            .Send(Arg.Any<GetProjectSprintQuery>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetProjectSprintQuery>(query => Utils.ProjectSprint.AssertGetQuery(query, request)));

        var response = outcome.ToResponse<GetProjectSprintResponse>();
        Utils.ProjectSprint.AssertGetResponse(response, sprint);
    }

    [Fact]
    public async Task GetProjectSprint_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetProjectSprintRequest();

        var error = Errors.ProjectSprint.NotFound;
        _mediator
            .Send(Arg.Any<GetProjectSprintQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetProjectSprintQuery>(query => Utils.ProjectSprint.AssertGetQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollectionProjectSprint_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest();

        var result = ProjectSprintResultFactory.CreateGetCollectionProjectSprintResult();
        _mediator
            .Send(Arg.Any<GetCollectionProjectSprintQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectSprintQuery>(
                query => Utils.ProjectSprint.AssertGetCollectionQuery(query, request)));

        var response = outcome.ToResponse<GetCollectionProjectSprintResponse>();
        Utils.ProjectSprint.AssertGetCollectionResponse(response, result);
    }

    [Fact]
    public async Task GetCollectionProjectSprint_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionProjectSprintQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectSprintQuery>(
                query => Utils.ProjectSprint.AssertGetCollectionQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task CreateProjectSprint_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest();

        var result = ProjectSprintResultFactory.CreateCreateProjectSprintResult();
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

        var response = outcome.ToResponse<CreateProjectSprintResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task CreateProjectSprint_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest();

        var error = Errors.Project.NotFound;
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
    public async Task UpdateProjectSprint_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateProjectSprintRequest();

        var result = ProjectSprintResultFactory.CreateUpdateProjectSprintResult();
        _mediator
            .Send(Arg.Any<UpdateProjectSprintCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectSprintCommand>(
                command => Utils.ProjectSprint.AssertUpdateCommand(command, request)));

        var response = outcome.ToResponse<UpdateProjectSprintResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateProjectSprint_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateProjectSprintRequest();

        var error = Errors.ProjectSprint.NotFound;
        _mediator
            .Send(Arg.Any<UpdateProjectSprintCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectSprintCommand>(
                command => Utils.ProjectSprint.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task DeleteProjectSprint_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateDeleteProjectSprintRequest();

        var result = ProjectSprintResultFactory.CreateDeleteProjectSprintResult();
        _mediator
            .Send(Arg.Any<DeleteProjectSprintCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectSprintCommand>(command =>
                Utils.ProjectSprint.AssertDeleteCommand(command, request)));

        var response = outcome.ToResponse<DeleteProjectSprintResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteProjectSprint_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateDeleteProjectSprintRequest();

        var error = Errors.ProjectSprint.NotFound;
        _mediator
            .Send(Arg.Any<DeleteProjectSprintCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectSprintCommand>(command =>
                Utils.ProjectSprint.AssertDeleteCommand(command, request)));

        outcome.ValidateError(error);
    }
}