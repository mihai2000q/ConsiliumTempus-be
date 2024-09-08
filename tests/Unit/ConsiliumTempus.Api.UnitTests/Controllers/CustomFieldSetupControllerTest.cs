using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.AddToProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.MakeGlobal;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestData;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class CustomFieldSetupControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly CustomFieldSetupController _uut;

    public CustomFieldSetupControllerTest()
    {
        var mapper = Utils.GetMapper<CustomFieldSetupMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new CustomFieldSetupController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Theory]
    [ClassData(typeof(CustomFieldSetupControllerData.GetCustomFieldSetupResults))]
    public async Task Get_WhenIsSuccessful_ShouldReturnResponse(GetCustomFieldSetupResult result)
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCustomFieldSetupRequest();

        _mediator
            .Send(Arg.Any<GetCustomFieldSetupQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCustomFieldSetupQuery>(q => 
                Utils.CustomFieldSetup.AssertGetCustomFieldSetupQuery(q, request)));

        var response = outcome.ToResponse<GetCustomFieldSetupResponse>();
        Utils.CustomFieldSetup.AssertGetCustomFieldSetupResponse(response, result);
    }

    [Fact]
    public async Task Get_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromProjectRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionCustomFieldSetupQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollectionFromProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionCustomFieldSetupQuery>(q =>
                Utils.CustomFieldSetup.AssertGetCollectionCustomFieldSetupQuery(q, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task GetCollectionFromProject_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromProjectRequest();

        var result = CustomFieldSetupResultFactory.CreateGetCollectionCustomFieldSetupResult();
        _mediator
            .Send(Arg.Any<GetCollectionCustomFieldSetupQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollectionFromProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionCustomFieldSetupQuery>(q =>
                Utils.CustomFieldSetup.AssertGetCollectionCustomFieldSetupQuery(q, request)));

        var response = outcome.ToResponse<GetCollectionCustomFieldSetupFromProjectResponse>();
        Utils.CustomFieldSetup.AssertGetCollectionFromProjectResponse(response, result);
    }

    [Fact]
    public async Task GetCollectionFromProject_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromProjectRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionCustomFieldSetupQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollectionFromProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionCustomFieldSetupQuery>(q =>
                Utils.CustomFieldSetup.AssertGetCollectionCustomFieldSetupQuery(q, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task GetCollectionFromWorkspace_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromWorkspaceRequest();

        var result = CustomFieldSetupResultFactory.CreateGetCollectionCustomFieldSetupResult();
        _mediator
            .Send(Arg.Any<GetCollectionCustomFieldSetupQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollectionFromWorkspace(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionCustomFieldSetupQuery>(q =>
                Utils.CustomFieldSetup.AssertGetCollectionCustomFieldSetupQuery(q, request)));

        var response = outcome.ToResponse<GetCollectionCustomFieldSetupFromWorkspaceResponse>();
        Utils.CustomFieldSetup.AssertGetCollectionFromWorkspaceResponse(response, result);
    }

    [Fact]
    public async Task GetCollectionFromWorkspace_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromWorkspaceRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionCustomFieldSetupQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollectionFromWorkspace(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionCustomFieldSetupQuery>(q =>
                Utils.CustomFieldSetup.AssertGetCollectionCustomFieldSetupQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task CreateOnWorkspace_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest();

        var result = CustomFieldSetupResultFactory.CreateCreateCustomFieldSetupResult();
        _mediator
            .Send(Arg.Any<CreateCustomFieldSetupCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.CreateOnWorkspace(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateCustomFieldSetupCommand>(c =>
                Utils.CustomFieldSetup.AssertCreateCustomFieldSetupCommand(c, request)));

        var response = outcome.ToResponse<CreateCustomFieldSetupOnWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task CreateOnWorkspace_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<CreateCustomFieldSetupCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.CreateOnWorkspace(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateCustomFieldSetupCommand>(c =>
                Utils.CustomFieldSetup.AssertCreateCustomFieldSetupCommand(c, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task CreateOnProject_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest();

        var result = CustomFieldSetupResultFactory.CreateCreateCustomFieldSetupResult();
        _mediator
            .Send(Arg.Any<CreateCustomFieldSetupCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.CreateOnProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateCustomFieldSetupCommand>(c =>
                Utils.CustomFieldSetup.AssertCreateCustomFieldSetupCommand(c, request)));

        var response = outcome.ToResponse<CreateCustomFieldSetupOnProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task CreateOnProject_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<CreateCustomFieldSetupCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.CreateOnProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateCustomFieldSetupCommand>(c =>
                Utils.CustomFieldSetup.AssertCreateCustomFieldSetupCommand(c, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task AddToProject_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateAddCustomFieldSetupToProjectRequest();

        var result = CustomFieldSetupResultFactory.CreateAddCustomFieldSetupToProjectResult();
        _mediator
            .Send(Arg.Any<AddCustomFieldSetupToProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.AddToProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AddCustomFieldSetupToProjectCommand>(c =>
                Utils.CustomFieldSetup.AssertAddCustomFieldSetupToProjectCommand(c, request)));

        var response = outcome.ToResponse<AddCustomFieldSetupToProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task AddToProject_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateAddCustomFieldSetupToProjectRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<AddCustomFieldSetupToProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.AddToProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AddCustomFieldSetupToProjectCommand>(c =>
                Utils.CustomFieldSetup.AssertAddCustomFieldSetupToProjectCommand(c, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Update_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest();

        var result = CustomFieldSetupResultFactory.CreateUpdateCustomFieldSetupResult();
        _mediator
            .Send(Arg.Any<UpdateCustomFieldSetupCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateCustomFieldSetupCommand>(c =>
                Utils.CustomFieldSetup.AssertUpdateCustomFieldSetupCommand(c, request)));

        var response = outcome.ToResponse<UpdateCustomFieldSetupResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Update_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<UpdateCustomFieldSetupCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateCustomFieldSetupCommand>(c =>
                Utils.CustomFieldSetup.AssertUpdateCustomFieldSetupCommand(c, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task UpdateWorkspace_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateMakeCustomFieldSetupGlobalRequest();

        var result = CustomFieldSetupResultFactory.CreateMakeCustomFieldSetupGlobalResult();
        _mediator
            .Send(Arg.Any<MakeCustomFieldSetupGlobalCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.MakeGlobal(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<MakeCustomFieldSetupGlobalCommand>(c =>
                Utils.CustomFieldSetup.AssertMakeCustomFieldSetupGlobalCommand(c, request)));

        var response = outcome.ToResponse<MakeCustomFieldGlobalResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateWorkspace_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateMakeCustomFieldSetupGlobalRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<MakeCustomFieldSetupGlobalCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.MakeGlobal(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<MakeCustomFieldSetupGlobalCommand>(c =>
                Utils.CustomFieldSetup.AssertMakeCustomFieldSetupGlobalCommand(c, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Delete_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateDeleteCustomFieldSetupRequest();

        var result = CustomFieldSetupResultFactory.CreateDeleteCustomFieldSetupResult();
        _mediator
            .Send(Arg.Any<DeleteCustomFieldSetupCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteCustomFieldSetupCommand>(c =>
                Utils.CustomFieldSetup.AssertDeleteCustomFieldSetupCommand(c, request)));

        var response = outcome.ToResponse<DeleteCustomFieldSetupResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Delete_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateDeleteCustomFieldSetupRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<DeleteCustomFieldSetupCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteCustomFieldSetupCommand>(c =>
                Utils.CustomFieldSetup.AssertDeleteCustomFieldSetupCommand(c, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task RemoveFromProject_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateRemoveCustomFieldSetupFromProjectRequest();

        var result = CustomFieldSetupResultFactory.CreateRemoveCustomFieldSetupFromProjectResult();
        _mediator
            .Send(Arg.Any<RemoveCustomFieldSetupFromProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.RemoveFromProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RemoveCustomFieldSetupFromProjectCommand>(c =>
                Utils.CustomFieldSetup.AssertRemoveCustomFieldSetupFromProjectCommand(c, request)));

        var response = outcome.ToResponse<RemoveCustomFieldSetupFromProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task RemoveFromProject_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateRemoveCustomFieldSetupFromProjectRequest();

        var error = Errors.CustomFieldSetup.NotFound;
        _mediator
            .Send(Arg.Any<RemoveCustomFieldSetupFromProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.RemoveFromProject(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RemoveCustomFieldSetupFromProjectCommand>(c =>
                Utils.CustomFieldSetup.AssertRemoveCustomFieldSetupFromProjectCommand(c, request)));

        outcome.ValidateError(error);
    }
}