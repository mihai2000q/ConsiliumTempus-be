using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;

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
    public async Task WhenLoginIsSuccessful_ShouldQueryUserAndReturnNewTokens()
    {
        // Arrange
        var query = AuthenticationCommandFactory.CreateLoginCommand();

        var user = UserFactory.Create(password: "This is the pass for Password123");
        _userRepository
            .GetByEmail(query.Email.ToLower())
            .Returns(user);

        _scrambler
            .VerifyPassword(query.Password, user.Credentials.Password)
            .Returns(true);

        const string token = "This is a token";
        _jwtTokenGenerator
            .GenerateToken(user)
            .Returns(token);

        var jwtId = new Guid().ToString();
        _jwtTokenGenerator
            .GetJwtIdFromToken(token)
            .Returns(jwtId);

        RefreshToken refreshToken = null!;
        _refreshTokenRepository
            .When(r => r.Add(Arg.Any<RefreshToken>()))
            .Do(rt => refreshToken = rt.Arg<RefreshToken>());
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Any<string>());
        _scrambler
            .Received(1)
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());
        _jwtTokenGenerator
            .Received(1)
            .GenerateToken(Arg.Any<UserAggregate>());
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Any<string>());
        await _refreshTokenRepository
            .Received(1)
            .Add(Arg.Any<RefreshToken>());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);
        
        Utils.RefreshToken.AssertCreation(refreshToken, jwtId, user);
        outcome.Value.RefreshToken.Should().Be(refreshToken.Value);
    }

    [Fact]
    public async Task WhenLoginFailsDueToMissingUser_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var query = AuthenticationCommandFactory.CreateLoginCommand();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Any<string>());
        _scrambler.DidNotReceive();
        _jwtTokenGenerator.DidNotReceive();

        outcome.ValidateError(Errors.Authentication.InvalidCredentials);
    }

    [Fact]
    public async Task WhenLoginFailsDueToWrongPassword_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var query = AuthenticationCommandFactory.CreateLoginCommand();

        var user = UserFactory.Create();
        _userRepository
            .GetByEmail(query.Email.ToLower())
            .Returns(user);

        _scrambler
            .VerifyPassword(query.Password, user.Credentials.Password)
            .Returns(false);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .GetByEmail(Arg.Any<string>());
        _scrambler
            .Received(1)
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());
        _jwtTokenGenerator.DidNotReceive();

        outcome.ValidateError(Errors.Authentication.InvalidCredentials);
    }
}