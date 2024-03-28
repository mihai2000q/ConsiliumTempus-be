using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authentication;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authentication;

public class JwtTokenValidatorTest
{
    #region Setup

    private readonly JwtSettings _jwtSettings;
    private readonly IUserProvider _userProvider;
    private readonly JwtTokenValidator _uut;

    public JwtTokenValidatorTest()
    {
        _jwtSettings = new JwtSettings
        {
            SecretKey = "This-Is-My-Secret-Duper-Key-And-I-Like-It",
            Audience = "Audience",
            ExpiryMinutes = 7,
            Issuer = "Issuer"
        };

        var options = Substitute.For<IOptions<JwtSettings>>();
        options
            .Value
            .Returns(_jwtSettings);
        
        _userProvider = Substitute.For<IUserProvider>();
        _uut = new JwtTokenValidator(options, _userProvider);
    }

    #endregion

    [Fact]
    public void ValidateRefreshToken_WhenValid_ShouldReturnTrue()
    {
        // Arrange
        var refreshToken = RefreshTokenFactory.Create();
        
        // Act
        var outcome = _uut.ValidateRefreshToken(refreshToken, refreshToken.JwtId.ToString());

        // Assert
        outcome.Should().BeTrue();
    }
    
    [Fact]
    public void ValidateRefreshToken_WhenRefreshTokenIsNull_ShouldReturnFalse()
    {
        // Arrange
        
        // Act
        var outcome = _uut.ValidateRefreshToken(null, "");

        // Assert
        outcome.Should().BeFalse();
    }
    
    [Fact]
    public void ValidateRefreshToken_WhenRefreshTokenIsInvalidated_ShouldReturnFalse()
    {
        // Arrange
        var refreshToken = RefreshTokenFactory.Create(invalidated: true);
        
        // Act
        var outcome = _uut.ValidateRefreshToken(refreshToken, refreshToken.JwtId.ToString());

        // Assert
        outcome.Should().BeFalse();
    }
    
    [Fact]
    public void ValidateRefreshToken_WhenRefreshTokenIsExpired_ShouldReturnFalse()
    {
        // Arrange
        var refreshToken = RefreshTokenFactory.Create(expiryDate: DateTime.UtcNow.AddSeconds(-1));
        
        // Act
        var outcome = _uut.ValidateRefreshToken(refreshToken, refreshToken.JwtId.ToString());

        // Assert
        outcome.Should().BeFalse();
    }
    
    [Fact]
    public void ValidateRefreshToken_WhenJwtIdIsWrong_ShouldReturnFalse()
    {
        // Arrange
        var refreshToken = RefreshTokenFactory.Create();
        
        // Act
        var outcome = _uut.ValidateRefreshToken(refreshToken, "wrong");

        // Assert
        outcome.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAccessToken_WhenValid_ShouldReturnTrue()
    {
        // Arrange
        var user = UserFactory.Create();
        _userProvider
            .Get(user.Id)
            .Returns(user);
        
        var token = Utils.Token.GenerateToken(_jwtSettings, user);
        
        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        await _userProvider
            .Received(1)
            .Get(Arg.Any<UserId>());
        
        outcome.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAccessToken_WhenUserIsNotFound_ShouldReturnFalse()
    {
        // Arrange
        var token = Utils.Token.GenerateToken(_jwtSettings);
        
        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        await _userProvider
            .Received(1)
            .Get(Arg.Any<UserId>());
        
        outcome.Should().BeFalse();
    }
    
    [Theory]
    [ClassData(typeof(JwtTokenValidatorData.GetInvalidTokensByJwtSettings))]
    public async Task ValidateAccessToken_WhenInvalidByJwtSettings_ShouldReturnFalse(string token)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        _userProvider.Received(0);
        
        outcome.Should().BeFalse();
    }
    
    [Fact]
    public async Task ValidateAccessToken_WhenUserIdIsWrong_ShouldReturnFalse()
    {
        // Arrange
        var token = Utils.Token.GenerateToken(_jwtSettings, sub: "asd");
        
        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        _userProvider.Received(0);
        
        outcome.Should().BeFalse();
    }
    
    [Theory]
    [ClassData(typeof(JwtTokenValidatorData.GetInvalidTokensByClaims))]
    public async Task ValidateAccessToken_WhenInvalidByClaims_ShouldReturnFalse(string token, UserAggregate user)
    {
        // Arrange - parameterized
        _userProvider
            .Get(user.Id)
            .Returns(user);
        
        // Act
        var outcome = await _uut.ValidateAccessToken(token);

        // Assert
        await _userProvider
            .Received(1)
            .Get(Arg.Any<UserId>());
        
        outcome.Should().BeFalse();
    }
}