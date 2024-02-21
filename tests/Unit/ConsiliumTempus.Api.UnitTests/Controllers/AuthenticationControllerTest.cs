using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class AuthenticationControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly AuthenticationController _uut;

    public AuthenticationControllerTest()
    {
        var mapper = Utils.GetMapper<AuthenticationMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new AuthenticationController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task WhenRegisterIsSuccessful_ShouldReturnRegisterResponse()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRegisterRequest();

        var result = new RegisterResult("This is the token for the registration");
        _mediator
            .Send(Arg.Any<RegisterCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Register(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RegisterCommand>(
                command => Utils.Authentication.AssertRegisterCommand(command, request)));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<RegisterResponse>();

        var response = ((OkObjectResult)outcome).Value as RegisterResponse;
        response?.Token.Should().Be(result.Token);
    }

    [Fact]
    public async Task WhenRegisterFails_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRegisterRequest();

        var error = Errors.User.DuplicateEmail;
        _mediator
            .Send(Arg.Any<RegisterCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Register(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RegisterCommand>(
                command => Utils.Authentication.AssertRegisterCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task WhenLoginIsSuccessful_ShouldReturnLoginResponse()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest();

        var result = new LoginResult("This is the token");
        _mediator
            .Send(Arg.Any<LoginQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.Login(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<LoginQuery>(
                query => Utils.Authentication.AssertLoginQuery(query, request)));

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<LoginResponse>();

        var response = ((OkObjectResult)outcome).Value as LoginResponse;
        response?.Token.Should().Be(result.Token);
    }

    [Fact]
    public async Task WhenLoginFails_ShouldReturnLoginResponse()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest();
        
        var error = Errors.Authentication.InvalidCredentials;
        _mediator
            .Send(Arg.Any<LoginQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.Login(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<LoginQuery>(
                query => Utils.Authentication.AssertLoginQuery(query, request)));

        outcome.ValidateError(error);
    }
}