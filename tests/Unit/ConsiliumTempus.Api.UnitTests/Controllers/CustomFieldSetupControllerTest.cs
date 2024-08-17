using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestData;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
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
        Utils.CustomFieldSetup.AssertGetCollectionCustomFieldSetupFromProjectResponse(response, result);
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
}