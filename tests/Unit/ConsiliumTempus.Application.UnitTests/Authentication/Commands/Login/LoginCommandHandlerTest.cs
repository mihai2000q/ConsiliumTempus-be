using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands.Login;

public class LoginCommandHandlerTest
{
    #region Setup

    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IScrambler _scrambler;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly LoginCommandHandler _uut;

    public LoginCommandHandlerTest()
    {
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _scrambler = Substitute.For<IScrambler>();
        _userRepository = Substitute.For<IUserRepository>();
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _uut = new LoginCommandHandler(
            _userRepository,
            _refreshTokenRepository,
            _scrambler,
            _jwtTokenGenerator);
    }

    #endregion

    [Fact]
    public async Task HandleLoginCommand_WhenIsSuccessful_ShouldQueryUserAndReturnNewTokens()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateLoginCommand();

        var user = UserFactory.Create();
        _userRepository
            .GetByEmail(Arg.Any<string>())
            .Returns(user);

        _scrambler
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>())
            .Returns(true);

        const string token = "This is a token";
        _jwtTokenGenerator
            .GenerateToken(Arg.Any<UserAggregate>())
            .Returns(token);

        var jwtId = Guid.NewGuid();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Any<string>())
            .Returns(jwtId);

        RefreshToken refreshToken = null!;
        _refreshTokenRepository
            .When(r => r.Add(Arg.Any<RefreshToken>()))
            .Do(rt => refreshToken = rt.Arg<RefreshToken>());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Is<string>(email => email == command.Email.ToLower()));
        _scrambler
            .Received(1)
            .VerifyPassword(
                Arg.Is<string>(p => p == command.Password),
                Arg.Is<string>(hp => hp == user.Credentials.Password));
        _jwtTokenGenerator
            .Received(1)
            .GenerateToken(Arg.Is<UserAggregate>(u => u == user));
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Is<string>(t => t == token));
        await _refreshTokenRepository
            .Received(1)
            .Add(Arg.Is<RefreshToken>(rt => rt == refreshToken));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);
        outcome.Value.RefreshToken.Should().Be(refreshToken.Id.Value);

        Utils.RefreshToken.AssertCreation(refreshToken, jwtId, user);
    }

    [Fact]
    public async Task HandleLoginCommand_WhenPasswordIsWrong_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateLoginCommand();

        var user = UserFactory.Create();
        _userRepository
            .GetByEmail(Arg.Any<string>())
            .Returns(user);

        _scrambler
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>())
            .Returns(false);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Is<string>(email => email == command.Email.ToLower()));
        _scrambler
            .Received(1)
            .VerifyPassword(
                Arg.Is<string>(p => p == command.Password),
                Arg.Is<string>(hp => hp == user.Credentials.Password));
        _jwtTokenGenerator.DidNotReceive();
        _refreshTokenRepository.DidNotReceive();

        outcome.ValidateError(Errors.Authentication.InvalidCredentials);
    }

    [Fact]
    public async Task HandleLoginCommand_WhenUserIsNull_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateLoginCommand();

        _userRepository
            .GetByEmail(Arg.Any<string>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Is<string>(email => email == command.Email.ToLower()));
        _scrambler.DidNotReceive();
        _jwtTokenGenerator.DidNotReceive();
        _refreshTokenRepository.DidNotReceive();

        outcome.ValidateError(Errors.Authentication.InvalidCredentials);
    }
}