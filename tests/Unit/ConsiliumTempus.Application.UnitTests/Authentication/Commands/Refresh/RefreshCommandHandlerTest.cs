using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using NSubstitute.ReturnsExtensions;

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
    public async Task HandleRefreshCommand_WhenIsValid_ShouldRefreshAndReturnNewAccessToken()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(Arg.Any<string>())
            .Returns(true);

        var jwtId = Guid.NewGuid();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Is(command.Token))
            .Returns(jwtId);

        var refreshToken = RefreshTokenFactory.Create(jwtId);
        _refreshTokenRepository
            .Get(Arg.Any<RefreshTokenId>())
            .Returns(refreshToken);

        _jwtTokenValidator
            .ValidateRefreshToken(Arg.Any<RefreshToken>())
            .Returns(true);

        const string newToken = "This is the result token";
        _jwtTokenGenerator
            .GenerateToken(Arg.Any<UserAggregate>())
            .Returns(newToken);

        var newJwtId = Guid.NewGuid();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Is(newToken))
            .Returns(newJwtId);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Is<string>(t => t == command.Token));
        _jwtTokenValidator
            .Received(1)
            .ValidateRefreshToken(Arg.Is<RefreshToken>(rt => rt == refreshToken));

        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Is<RefreshTokenId>(id => id.Value == command.RefreshToken));

        _jwtTokenGenerator
            .Received(2)
            .GetJwtIdFromToken(Arg.Any<string>());
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Is<string>(t => t == command.Token));
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Is<string>(t => t == newToken));

        _jwtTokenGenerator
            .Received(1)
            .GenerateToken(Arg.Is<UserAggregate>(u => u == refreshToken.User));
        _jwtTokenGenerator
            .DidNotReceive()
            .GenerateToken(Arg.Any<UserAggregate>(), Arg.Any<Guid>());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(newToken);

        Utils.RefreshToken.AssertRefresh(refreshToken, newJwtId);
    }
    
    [Fact]
    public async Task HandleRefreshCommand_WhenItHasBeenRefreshedAlready_ShouldReturnAccessToken()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(Arg.Any<string>())
            .Returns(true);

        var jwtId = Guid.NewGuid();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Is(command.Token))
            .Returns(jwtId);

        var currentJwtId = Guid.NewGuid();
        var refreshToken = RefreshTokenFactory.Create(jwtId);
        refreshToken.Refresh(JwtId.Create(currentJwtId));
        _refreshTokenRepository
            .Get(Arg.Any<RefreshTokenId>())
            .Returns(refreshToken);

        _jwtTokenValidator
            .ValidateRefreshToken(Arg.Any<RefreshToken>())
            .Returns(true);

        const string token = "This is the result token";
        _jwtTokenGenerator
            .GenerateToken(Arg.Any<UserAggregate>(), Arg.Any<Guid>())
            .Returns(token);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Is<string>(t => t == command.Token));
        _jwtTokenValidator
            .Received(1)
            .ValidateRefreshToken(Arg.Is<RefreshToken>(rt => rt == refreshToken));

        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Is<RefreshTokenId>(id => id.Value == command.RefreshToken));

        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Any<string>());
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Is<string>(t => t == command.Token));

        _jwtTokenGenerator
            .Received(1)
            .GenerateToken(
                Arg.Is<UserAggregate>(u => u == refreshToken.User),
                Arg.Is<Guid>(jti => jti == currentJwtId));
        _jwtTokenGenerator
            .DidNotReceive()
            .GenerateToken(Arg.Any<UserAggregate>());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);
    }
    
    [Fact]
    public async Task HandleRefreshCommand_WhenJwtIdIsUnknown_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(Arg.Any<string>())
            .Returns(true);

        var jwtId = Guid.NewGuid();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Is(command.Token))
            .Returns(jwtId);

        var refreshToken = RefreshTokenFactory.Create();
        refreshToken.Refresh(JwtId.Create(Guid.NewGuid()));
        _refreshTokenRepository
            .Get(Arg.Any<RefreshTokenId>())
            .Returns(refreshToken);

        _jwtTokenValidator
            .ValidateRefreshToken(Arg.Any<RefreshToken>())
            .Returns(true);

        const string token = "This is the result token";
        _jwtTokenGenerator
            .GenerateToken(Arg.Any<UserAggregate>(), Arg.Any<Guid>())
            .Returns(token);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Is<string>(t => t == command.Token));
        _jwtTokenValidator
            .Received(1)
            .ValidateRefreshToken(Arg.Is<RefreshToken>(rt => rt == refreshToken));

        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Is<RefreshTokenId>(id => id.Value == command.RefreshToken));

        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Any<string>());
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Is<string>(t => t == command.Token));

        _jwtTokenGenerator
            .DidNotReceive()
            .GenerateToken(Arg.Any<UserAggregate>());
        _jwtTokenGenerator
            .DidNotReceive()
            .GenerateToken(Arg.Any<UserAggregate>(), Arg.Any<Guid>());

        outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }

    [Fact]
    public async Task HandleRefreshCommand_WhenRefreshTokenIsInvalid_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(Arg.Any<string>())
            .Returns(true);

        var jwtId = Guid.NewGuid();
        _jwtTokenGenerator
            .GetJwtIdFromToken(Arg.Is(command.Token))
            .Returns(jwtId);

        var refreshToken = RefreshTokenFactory.Create(jwtId);
        _refreshTokenRepository
            .Get(Arg.Any<RefreshTokenId>())
            .Returns(refreshToken);

        _jwtTokenValidator
            .ValidateRefreshToken(Arg.Any<RefreshToken>())
            .Returns(false);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Is<string>(t => t == command.Token));
        _jwtTokenValidator
            .Received(1)
            .ValidateRefreshToken(Arg.Is<RefreshToken>(rt => refreshToken == rt));

        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Is<RefreshTokenId>(id => id.Value == command.RefreshToken));

        _jwtTokenGenerator.DidNotReceive();

        outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task HandleRefreshCommand_WhenRefreshTokenIsNull_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(Arg.Any<string>())
            .Returns(true);
        
        _refreshTokenRepository
            .Get(Arg.Any<RefreshTokenId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Is<string>(t => t == command.Token));
        
        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Is<RefreshTokenId>(id => id.Value == command.RefreshToken));

        _jwtTokenValidator
            .DidNotReceive()
            .ValidateRefreshToken(Arg.Any<RefreshToken>());

        _refreshTokenRepository.DidNotReceive();
        _jwtTokenGenerator.DidNotReceive();

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
            .ValidateRefreshToken(Arg.Any<RefreshToken>());

        _refreshTokenRepository.DidNotReceive();
        _jwtTokenGenerator.DidNotReceive();

        outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
}