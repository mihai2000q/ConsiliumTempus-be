using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands.Refresh;

public class RefreshCommandHandlerTest
{
    #region Setup

    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IJwtTokenValidator _jwtTokenValidator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly RefreshCommandHandler _uut;

    public RefreshCommandHandlerTest()
    {
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _jwtTokenValidator = Substitute.For<IJwtTokenValidator>();
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _uut = new RefreshCommandHandler(
            _jwtTokenValidator,
            _jwtTokenGenerator,
            _refreshTokenRepository);
    }

    #endregion

    [Fact]
    public async Task HandleRefreshCommand_WhenIsValid_ShouldReturnNewToken()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(Arg.Any<string>())
            .Returns(true);

        var jwtId = Guid.NewGuid().ToString();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Is(command.Token))
            .Returns(jwtId);

        var refreshToken = RefreshTokenFactory.Create(jwtId);
        _refreshTokenRepository
            .Get(Arg.Any<Guid>())
            .Returns(refreshToken);

        _jwtTokenValidator
            .ValidateRefreshToken(Arg.Any<RefreshToken?>(), Arg.Any<string>())
            .Returns(true);

        const string token = "This is the result token";
        _jwtTokenGenerator
            .GenerateToken(Arg.Any<UserAggregate>())
            .Returns(token);

        var newJwtId = Guid.NewGuid().ToString();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Is<string>(token))
            .Returns(newJwtId);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Is<string>(t => t == command.Token));
        _jwtTokenValidator
            .Received(1)
            .ValidateRefreshToken(
                Arg.Is<RefreshToken?>(rt => refreshToken.Equals(rt)),
                Arg.Is<string>(jId => jId == jwtId));

        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Is<Guid>(id => id.ToString() == command.RefreshToken));

        _jwtTokenGenerator
            .Received(2)
            .GetJwtIdFromToken(Arg.Any<string>());
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Is<string>(t => t == command.Token));
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Is<string>(t => t == token));

        _jwtTokenGenerator
            .Received(1)
            .GenerateToken(Arg.Is<UserAggregate>(u => u == refreshToken.User));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);

        Utils.RefreshToken.AssertUpdate(refreshToken, newJwtId);
    }

    [Fact]
    public async Task HandleRefreshCommand_WhenRefreshTokenIsInvalid_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(Arg.Any<string>())
            .Returns(true);

        var jwtId = Guid.NewGuid().ToString();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Is(command.Token))
            .Returns(jwtId);

        var refreshToken = RefreshTokenFactory.Create(jwtId);
        _refreshTokenRepository
            .Get(Arg.Any<Guid>())
            .Returns(refreshToken);

        _jwtTokenValidator
            .ValidateRefreshToken(Arg.Any<RefreshToken?>(), Arg.Any<string>())
            .Returns(false);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Is<string>(t => t == command.Token));
        _jwtTokenValidator
            .Received(1)
            .ValidateRefreshToken(
                Arg.Is<RefreshToken?>(rt => refreshToken.Equals(rt)),
                Arg.Is<string>(jId => jId == jwtId));

        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Is<Guid>(id => id.ToString() == command.RefreshToken));

        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Any<string>());
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Is<string>(t => t == command.Token));

        _jwtTokenGenerator
            .DidNotReceive()
            .GenerateToken(Arg.Is<UserAggregate>(u => u == refreshToken.User));

        outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }

    [Fact]
    public async Task HandleRefreshCommand_WhenAccessTokenIsInvalid_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(Arg.Any<string>())
            .Returns(false);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Is<string>(t => t == command.Token));

        _jwtTokenValidator
            .DidNotReceive()
            .ValidateRefreshToken(Arg.Any<RefreshToken?>(), Arg.Any<string>());

        _refreshTokenRepository.DidNotReceive();
        _jwtTokenGenerator.DidNotReceive();

        outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
}