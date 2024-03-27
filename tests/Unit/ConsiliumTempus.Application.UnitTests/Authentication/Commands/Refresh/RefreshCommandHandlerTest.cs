using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

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
    public async Task RefreshCommand_WhenIsValid_ShouldReturnNewToken()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(command.Token)
            .Returns(true);

        var jwtId = Guid.NewGuid().ToString();
        _jwtTokenGenerator
            .GetJwtIdFromToken(command.Token)
            .Returns(jwtId);
        
        var refreshToken = RefreshTokenFactory.Create(jwtId);
        _refreshTokenRepository
            .Get(command.RefreshToken)
            .Returns(refreshToken);
        
        _jwtTokenValidator
            .ValidateRefreshToken(refreshToken, jwtId)
            .Returns(true);

        const string token = "This is the result token";
        _jwtTokenGenerator
            .GenerateToken(refreshToken.User)
            .Returns(token);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Any<string>());
        _jwtTokenValidator
            .Received(1)
            .ValidateRefreshToken(Arg.Any<RefreshToken?>(), Arg.Any<string>());

        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Any<string>());
        
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Any<string>());
        _jwtTokenGenerator
            .Received(1)
            .GenerateToken(Arg.Any<UserAggregate>());
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);

        refreshToken.UsedTimes.Should().Be(1);
        refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
    }
    
    [Fact]
    public async Task RefreshCommand_WhenRefreshTokenIsInvalid_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(command.Token)
            .Returns(true);

        var jwtId = Guid.NewGuid().ToString();
        _jwtTokenGenerator
            .GetJwtIdFromToken(command.Token)
            .Returns(jwtId);
        
        var refreshToken = RefreshTokenFactory.Create(jwtId);
        _refreshTokenRepository
            .Get(command.RefreshToken)
            .Returns(refreshToken);
        
        _jwtTokenValidator
            .ValidateRefreshToken(refreshToken, jwtId)
            .Returns(false);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Any<string>());
        _jwtTokenValidator
            .Received(1)
            .ValidateRefreshToken(Arg.Any<RefreshToken?>(), Arg.Any<string>());

        await _refreshTokenRepository
            .Received(1)
            .Get(Arg.Any<string>());
        
        _jwtTokenGenerator
            .Received(1)
            .GetJwtIdFromToken(Arg.Any<string>());
        _jwtTokenGenerator
            .Received(0)
            .GenerateToken(Arg.Any<UserAggregate>());
        
        outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task RefreshCommand_WhenAccessTokenIsInvalid_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var command = AuthenticationCommandFactory.CreateRefreshCommand();

        _jwtTokenValidator
            .ValidateAccessToken(command.Token)
            .Returns(false);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _jwtTokenValidator
            .Received(1)
            .ValidateAccessToken(Arg.Any<string>());
        
        _jwtTokenValidator
            .Received(0)
            .ValidateRefreshToken(Arg.Any<RefreshToken?>(), Arg.Any<string>());

        _refreshTokenRepository.Received(0);
        _jwtTokenGenerator.Received(0);
        
        outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
}