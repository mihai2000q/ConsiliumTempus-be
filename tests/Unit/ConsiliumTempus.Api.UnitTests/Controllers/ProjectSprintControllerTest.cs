using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Delete;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestData;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
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
    public async Task Get_WhenIsSuccessful_ShouldReturnResponse()
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
    public async Task Get_WhenItFails_ShouldReturnProblem()
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
    public async Task GetCollection_WhenIsSuccessful_ShouldReturnResponse()
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
    public async Task GetCollection_WhenItFails_ShouldReturnProblem()
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

    [Theory]
    [ClassData(typeof(ProjectSprintControllerData.GetCreateProjectSprintRequests))]
    public async Task Create_WhenIsSuccessful_ShouldReturnResponse(CreateProjectSprintRequest request)
    {
        // Arrange - parameterized
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
    public async Task Create_WhenItFails_ShouldReturnProblem()
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
    public async Task AddStage_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateAddStageToProjectSprintRequest();

        var result = ProjectSprintResultFactory.CreateAddStageToProjectSprintResult();
        _mediator
            .Send(Arg.Any<AddStageToProjectSprintCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.AddStage(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AddStageToProjectSprintCommand>(
                command => Utils.ProjectSprint.AssertAddStageCommand(command, request)));

        var response = outcome.ToResponse<AddStageToProjectSprintResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task AddStage_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateAddStageToProjectSprintRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<AddStageToProjectSprintCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.AddStage(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AddStageToProjectSprintCommand>(
                command => Utils.ProjectSprint.AssertAddStageCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Update_WhenIsSuccessful_ShouldReturnResponse()
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
    public async Task Update_WhenItFails_ShouldReturnProblem()
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
    public async Task UpdateStage_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateStageToProjectSprintRequest();

        var result = ProjectSprintResultFactory.CreateUpdateStageFromProjectSprintResult();
        _mediator
            .Send(Arg.Any<UpdateStageFromProjectSprintCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateStage(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateStageFromProjectSprintCommand>(
                command => Utils.ProjectSprint.AssertUpdateStageCommand(command, request)));

        var response = outcome.ToResponse<UpdateStageFromProjectSprintResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateStage_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateStageToProjectSprintRequest();

        var error = Errors.ProjectSprint.NotFound;
        _mediator
            .Send(Arg.Any<UpdateStageFromProjectSprintCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateStage(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateStageFromProjectSprintCommand>(
                command => Utils.ProjectSprint.AssertUpdateStageCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Delete_WhenIsSuccessful_ShouldReturnSuccess()
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
    public async Task Delete_WhenItFails_ShouldReturnProblem()
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
    
    [Fact]
    public async Task RemoveStage_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest();

        var result = ProjectSprintResultFactory.CreateRemoveStageFromProjectSprintResult();
        _mediator
            .Send(Arg.Any<RemoveStageFromProjectSprintCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.RemoveStage(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RemoveStageFromProjectSprintCommand>(command =>
                Utils.ProjectSprint.AssertRemoveStageCommand(command, request)));

        var response = outcome.ToResponse<RemoveStageFromProjectSprintResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task RemoveStage_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest();

        var error = Errors.ProjectSprint.NotFound;
        _mediator
            .Send(Arg.Any<RemoveStageFromProjectSprintCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.RemoveStage(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RemoveStageFromProjectSprintCommand>(command =>
                Utils.ProjectSprint.AssertRemoveStageCommand(command, request)));

        outcome.ValidateError(error);
    }
}