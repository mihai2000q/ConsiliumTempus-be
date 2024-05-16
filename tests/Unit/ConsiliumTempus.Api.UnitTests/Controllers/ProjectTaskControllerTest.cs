using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class ProjectTaskControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly ProjectTaskController _uut;

    public ProjectTaskControllerTest()
    {
        var mapper = Utils.GetMapper<ProjectTaskMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new ProjectTaskController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task Get_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateGetProjectTaskRequest();

        var task = ProjectTaskFactory.Create();
        _mediator
            .Send(Arg.Any<GetProjectTaskQuery>())
            .Returns(task);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetProjectTaskQuery>(q =>
                Utils.ProjectTask.AssertGetProjectTaskQuery(q, request)));

        var response = outcome.ToResponse<GetProjectTaskResponse>();
        Utils.ProjectTask.AssertGetProjectTaskResponse(response, task);
    }

    [Fact]
    public async Task Get_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateGetProjectTaskRequest();

        var error = Errors.ProjectTask.NotFound;
        _mediator
            .Send(Arg.Any<GetProjectTaskQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetProjectTaskQuery>(q =>
                Utils.ProjectTask.AssertGetProjectTaskQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollection_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest();

        var result = ProjectTaskResultFactory.CreateGetCollectionProjectTaskResult();
        _mediator
            .Send(Arg.Any<GetCollectionProjectTaskQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectTaskQuery>(q =>
                Utils.ProjectTask.AssertGetCollectionProjectTaskQuery(q, request)));

        var response = outcome.ToResponse<GetCollectionProjectTaskResponse>();
        Utils.ProjectTask.AssertGetCollectionResponse(response, result);
    }

    [Fact]
    public async Task GetCollection_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionProjectTaskQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectTaskQuery>(q =>
                Utils.ProjectTask.AssertGetCollectionProjectTaskQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Create_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateCreateProjectTaskRequest();

        var result = ProjectTaskResultFactory.CreateCreateProjectTaskResult();
        _mediator
            .Send(Arg.Any<CreateProjectTaskCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectTaskCommand>(command => Utils.ProjectTask.AssertCreateCommand(command, request)));

        var response = outcome.ToResponse<CreateProjectTaskResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Create_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateCreateProjectTaskRequest();

        var error = Errors.ProjectStage.NotFound;
        _mediator
            .Send(Arg.Any<CreateProjectTaskCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectTaskCommand>(command => Utils.ProjectTask.AssertCreateCommand(command, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task Update_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateProjectTaskRequest();

        var result = ProjectTaskResultFactory.CreateUpdateProjectTaskResult();
        _mediator
            .Send(Arg.Any<UpdateProjectTaskCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectTaskCommand>(command => 
                Utils.ProjectTask.AssertUpdateCommand(command, request)));

        var response = outcome.ToResponse<UpdateProjectTaskResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Update_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateProjectTaskRequest();

        var error = Errors.ProjectTask.NotFound;
        _mediator
            .Send(Arg.Any<UpdateProjectTaskCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectTaskCommand>(command => 
                Utils.ProjectTask.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task UpdateOverview_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateOverviewProjectTaskRequest();

        var result = ProjectTaskResultFactory.CreateUpdateOverviewProjectTaskResult();
        _mediator
            .Send(Arg.Any<UpdateOverviewProjectTaskCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateOverviewProjectTaskCommand>(command => 
                Utils.ProjectTask.AssertUpdateOverviewCommand(command, request)));

        var response = outcome.ToResponse<UpdateOverviewProjectTaskResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateOverview_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateOverviewProjectTaskRequest();

        var error = Errors.ProjectTask.NotFound;
        _mediator
            .Send(Arg.Any<UpdateOverviewProjectTaskCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateOverviewProjectTaskCommand>(command => 
                Utils.ProjectTask.AssertUpdateOverviewCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Delete_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateDeleteProjectTaskRequest();

        var result = ProjectTaskResultFactory.CreateDeleteProjectTaskResult();
        _mediator
            .Send(Arg.Any<DeleteProjectTaskCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectTaskCommand>(command =>
                Utils.ProjectTask.AssertDeleteCommand(command, request)));

        var response = outcome.ToResponse<DeleteProjectTaskResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Delete_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateDeleteProjectTaskRequest();

        var error = Errors.ProjectTask.NotFound;
        _mediator
            .Send(Arg.Any<DeleteProjectTaskCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectTaskCommand>(command =>
                Utils.ProjectTask.AssertDeleteCommand(command, request)));

        outcome.ValidateError(error);
    }
}