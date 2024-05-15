using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Refresh;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Domain.Common.Errors;

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

        var result = AuthenticationResultFactory.CreateRegisterResult();
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

        var response = outcome.ToResponse<RegisterResponse>();
        response.Token.Should().Be(result.Token);
        response.RefreshToken.Should().Be(result.RefreshToken);
    }

    [Fact]
    public async Task WhenRegisterFails_ShouldProblem()
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

        var result = AuthenticationResultFactory.CreateLoginResult();
        _mediator
            .Send(Arg.Any<LoginCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Login(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<LoginCommand>(
                query => Utils.Authentication.AssertLoginCommand(query, request)));

        var response = outcome.ToResponse<LoginResponse>();
        response.Token.Should().Be(result.Token);
        response.RefreshToken.Should().Be(result.RefreshToken);
    }

    [Fact]
    public async Task WhenLoginFails_ShouldReturnProblem()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest();

        var error = Errors.Authentication.InvalidCredentials;
        _mediator
            .Send(Arg.Any<LoginCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Login(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<LoginCommand>(
                query => Utils.Authentication.AssertLoginCommand(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task WhenRefreshIsSuccessful_ShouldReturnAccessToken()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest();

        var result = AuthenticationResultFactory.CreateRefreshResult();
        _mediator
            .Send(Arg.Any<RefreshCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Refresh(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RefreshCommand>(
                command => Utils.Authentication.AssertRefreshCommand(command, request)));

        var response = outcome.ToResponse<RefreshResponse>();
        response.Token.Should().Be(result.Token);
    }

    [Fact]
    public async Task WhenRefreshFails_ShouldReturnProblem()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest();

        var error = Errors.Authentication.InvalidTokens;
        _mediator
            .Send(Arg.Any<RefreshCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Refresh(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RefreshCommand>(
                command => Utils.Authentication.AssertRefreshCommand(command, request)));

        outcome.ValidateError(error);
    }
}