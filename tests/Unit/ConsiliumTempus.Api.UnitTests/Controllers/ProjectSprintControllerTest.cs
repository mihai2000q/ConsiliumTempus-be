using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;
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
    public async Task GetCollectionProjectSprint_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest();

        var result = new GetCollectionProjectSprintResult(ProjectSprintFactory.CreateList());
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
    public async Task GetCollectionProjectSprint_WhenProjectIsNotFound_ShouldReturnNotFoundError()
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

        var result = new CreateProjectSprintResult();
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
    public async Task CreateProjectSprint_WhenProjectIsNotFound_ShouldReturnNotFoundError()
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
    public async Task DeleteProjectSprint_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();

        var result = new DeleteProjectSprintResult();
        _mediator
            .Send(Arg.Any<DeleteProjectSprintCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectSprintCommand>(command =>
                Utils.ProjectSprint.AssertDeleteCommand(command, id)));

        var response = outcome.ToResponse<DeleteProjectSprintResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteProjectSprint_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        var error = Errors.ProjectSprint.NotFound;
        _mediator
            .Send(Arg.Any<DeleteProjectSprintCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectSprintCommand>(command =>
                Utils.ProjectSprint.AssertDeleteCommand(command, id)));

        outcome.ValidateError(error);
    }
}