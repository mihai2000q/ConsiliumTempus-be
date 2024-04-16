using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.User.DeleteCurrent;
using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.GetCurrent;
using ConsiliumTempus.Api.Contracts.User.UpdateCurrent;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.DeleteCurrent;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Application.User.Queries.GetCurrent;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class UserControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly UserController _uut;

    public UserControllerTest()
    {
        var mapper = Utils.GetMapper<UserMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new UserController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task GetUser_WhenIsSuccessful_ShouldReturnUser()
    {
        // Arrange
        var request = UserRequestFactory.CreateGetUserRequest();

        var user = UserFactory.Create();
        _mediator
            .Send(Arg.Any<GetUserQuery>())
            .Returns(user);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetUserQuery>(query => Utils.User.AssertGetQuery(query, request)));

        var response = outcome.ToResponse<GetUserResponse>();
        Utils.User.AssertGetUserResponse(response, user);
    }

    [Fact]
    public async Task GetUser_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = UserRequestFactory.CreateGetUserRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetUserQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetUserQuery>(query => Utils.User.AssertGetQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCurrent_WhenIsSuccessful_ShouldReturnCurrentUser()
    {
        // Arrange
        var user = UserFactory.Create();
        _mediator
            .Send(Arg.Any<GetCurrentUserQuery>())
            .Returns(user);

        // Act
        var outcome = await _uut.GetCurrent(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCurrentUserQuery>(query => query == new GetCurrentUserQuery()));

        var response = outcome.ToResponse<GetCurrentUserResponse>();
        Utils.User.AssertGetCurrentUserResponse(response, user);
    }

    [Fact]
    public async Task GetCurrent_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetCurrentUserQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCurrent(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCurrentUserQuery>(query => query == new GetCurrentUserQuery()));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task UpdateCurrentUser_WhenIsSuccessful_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateCurrentUserRequest();

        var result = new UpdateCurrentUserResult();
        _mediator
            .Send(Arg.Any<UpdateCurrentUserCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateCurrent(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateCurrentUserCommand>(command => Utils.User.AssertUpdateCommand(command, request)));

        var response = outcome.ToResponse<UpdateCurrentUserResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateCurrentUser_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateCurrentUserRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<UpdateCurrentUserCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateCurrent(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateCurrentUserCommand>(command => Utils.User.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task DeleteCurrentUser_WhenIsSuccessful_ShouldReturnSuccessResponse()
    {
        // Arrange
        var result = new DeleteCurrentUserResult();
        _mediator
            .Send(Arg.Any<DeleteCurrentUserCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.DeleteCurrent(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteCurrentUserCommand>(command => command == new DeleteCurrentUserCommand()));

        var response = outcome.ToResponse<DeleteCurrentUserResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task DeleteCurrentUser_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<DeleteCurrentUserCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.DeleteCurrent(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteCurrentUserCommand>(command => command == new DeleteCurrentUserCommand()));

        outcome.ValidateError(error);
    }
}