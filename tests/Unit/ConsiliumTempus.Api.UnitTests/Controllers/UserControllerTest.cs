using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.Delete;
using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class UserControllerTest
{
    #region Setup

    private readonly Mock<ISender> _mediator;
    private readonly UserController _uut;

    public UserControllerTest()
    {
        var mapper = Utils.GetMapper<AuthenticationMappingConfig>();

        _mediator = new Mock<ISender>();
        _uut = new UserController(mapper, _mediator.Object);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task UpdateUser_WhenIsSuccessful_ShouldReturnNewUser()
    {
        // Arrange
        var request = new UpdateUserRequest(
            Guid.NewGuid(),
            "New First Name",
            "New Last Name");

        var result = new UpdateUserResult(Mock.Mock.User.CreateMock());
        _mediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default))
            .ReturnsAsync(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        _mediator.Verify(m =>
                m.Send(It.Is<UpdateUserCommand>(
                        command => Utils.User.AssertUpdateCommand(command, request)),
                    default),
            Times.Once());

        Utils.User.AssertDto(outcome, result.User);
    }

    [Fact]
    public async Task UpdateUser_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = new UpdateUserRequest(
            Guid.NewGuid(),
            "New First Name",
            "New Last Name");

        var error = Errors.User.NotFound;
        _mediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default))
            .ReturnsAsync(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        _mediator.Verify(m =>
                m.Send(It.Is<UpdateUserCommand>(
                        command => Utils.User.AssertUpdateCommand(command, request)),
                    default),
            Times.Once());

        outcome.ValidateError(StatusCodes.Status404NotFound, error.Description);
    }

    [Fact]
    public async Task DeleteUser_WhenIsSuccessful_ShouldReturnUser()
    {
        // Arrange
        var id = Guid.NewGuid();


        var result = new DeleteUserResult(Mock.Mock.User.CreateMock());
        _mediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default))
            .ReturnsAsync(result);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        _mediator.Verify(m =>
                m.Send(It.Is<DeleteUserCommand>(
                        command => command.Id == id),
                    default),
            Times.Once());

        Utils.User.AssertDto(outcome, result.User);
    }

    [Fact]
    public async Task DeleteUser_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        var error = Errors.User.NotFound;
        _mediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default))
            .ReturnsAsync(error);

        // Act
        var outcome = await _uut.Delete(id, default);

        // Assert
        _mediator.Verify(m =>
                m.Send(It.Is<DeleteUserCommand>(
                        command => command.Id == id),
                    default),
            Times.Once());

        outcome.ValidateError(StatusCodes.Status404NotFound, error.Description);
    }
}