using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.User.Delete;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.Delete;
using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Application.User.Queries.GetCurrent;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

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

        Utils.User.AssertDto(outcome, user);
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

        Utils.User.AssertDto(outcome, user);
    }
    
    [Fact]
    public async Task GetCurrent_WhenItFails_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest();

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
    public async Task UpdateUser_WhenIsSuccessful_ShouldReturnNewUser()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest();

        var result = new UpdateUserResult(UserFactory.Create());
        _mediator
            .Send(Arg.Any<UpdateUserCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateUserCommand>(command => Utils.User.AssertUpdateCommand(command, request)));

        Utils.User.AssertDto(outcome, result.User);
    }
    
    [Fact]
    public async Task UpdateUser_WhenItFails_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<UpdateUserCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateUserCommand>(command => Utils.User.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task DeleteUser_WhenIsSuccessful_ShouldReturnUser()
    {
        // Arrange
        var result = new DeleteUserResult();
        _mediator
            .Send(Arg.Any<DeleteUserCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteUserCommand>(command => command == new DeleteUserCommand()));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<DeleteUserResponse>();

        var response = ((OkObjectResult)outcome).Value as DeleteUserResponse;
        response!.Message.Should().Be(result.Message);
    }
    
    [Fact]
    public async Task DeleteUser_WhenItFails_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<DeleteUserCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteUserCommand>(command => command == new DeleteUserCommand()));

        outcome.ValidateError(error);
    }
}