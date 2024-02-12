using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class AuthenticationControllerTest
{
    #region Setup

    private readonly Mock<ISender> _mediator;
    private readonly AuthenticationController _uut;

    public AuthenticationControllerTest()
    {
        var mapper = Utils.GetMapper<AuthenticationMappingConfig>();

        _mediator = new Mock<ISender>();
        _uut = new AuthenticationController(mapper, _mediator.Object);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task WhenRegisterIsSuccessful_ShouldReturnRegisterResponse()
    {
        // Arrange
        var request = new RegisterRequest(
            "FirstName",
            "LastName",
            "Example@Email.com",
            "Password123",
            null, 
            null);

        var result = new RegisterResult("This is the token for the registration");

        _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default))
            .ReturnsAsync(result);

        // Act
        var outcome = await _uut.Register(request, default);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<RegisterCommand>(command => Utils.Authentication.AssertRegisterCommand(command, request)),
                default),
            Times.Once());

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<RegisterResponse>();

        var response = ((OkObjectResult)outcome).Value as RegisterResponse;
        response?.Token.Should().Be(result.Token);
    }

    [Fact]
    public async Task WhenRegisterFails_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var request = new RegisterRequest(
            "FirstName",
            "LastName",
            "Example@Email.com",
            "Password123",
            null,
            null);

        _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default))
            .ReturnsAsync(Errors.User.DuplicateEmail);

        // Act
        var outcome = await _uut.Register(request, default);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<RegisterCommand>(command => Utils.Authentication.AssertRegisterCommand(command, request)),
                default),
            Times.Once());

        outcome.ValidateError(StatusCodes.Status409Conflict, "Email is already in use");
    }

    [Fact]
    public async Task WhenLoginIsSuccessful_ShouldReturnLoginResponse()
    {
        // Arrange
        var request = new LoginRequest(
            "Some@Example.com",
            "Password123");

        var result = new LoginResult("This is the token");
        _mediator.Setup(m => m.Send(It.IsAny<LoginQuery>(), default))
            .ReturnsAsync(result);

        // Act
        var outcome = await _uut.Login(request, default);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<LoginQuery>(query => Utils.Authentication.AssertLoginQuery(query, request)),
                default),
            Times.Once());

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<LoginResponse>();

        var response = ((OkObjectResult)outcome).Value as LoginResponse;
        response?.Token.Should().Be(result.Token);
    }

    [Fact]
    public async Task WhenLoginFails_ShouldReturnLoginResponse()
    {
        // Arrange
        var request = new LoginRequest(
            "Some@Example.com",
            "Password123");

        _mediator.Setup(m => m.Send(It.IsAny<LoginQuery>(), default))
            .ReturnsAsync(Errors.Authentication.InvalidCredentials);

        // Act
        var outcome = await _uut.Login(request, default);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<LoginQuery>(query => Utils.Authentication.AssertLoginQuery(query, request)),
                default),
            Times.Once());

        outcome.ValidateError(StatusCodes.Status401Unauthorized, "Invalid Credentials");
    }
}