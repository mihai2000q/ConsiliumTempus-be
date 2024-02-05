using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Common.Security;

public class SecurityTest
{
    #region Setup
    
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Application.Common.Security.Security _uut;
    
    public SecurityTest()
    {
        _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _userRepository = new Mock<IUserRepository>();
        _uut = new Application.Common.Security.Security(_jwtTokenGenerator.Object, _userRepository.Object);
    }

    #endregion

    [Fact]
    public async Task GetUserFromToken_WhenIsSuccessful_ShouldReturnUser()
    {
        // Arrange
        const string plainToken = "This is the user Token";

        var plainUserId = Guid.NewGuid().ToString();
        _jwtTokenGenerator.Setup(j => j.GetUserIdFromToken(plainToken))
            .Returns(plainUserId);

        var user = Mock.Mock.User.CreateMock();
        _userRepository.Setup(u => u.Get(It.IsAny<UserId>()))
            .ReturnsAsync(user);
        
        // Act
        var outcome = await _uut.GetUserFromToken(plainToken);

        // Assert
        _jwtTokenGenerator.Verify(j => j.GetUserIdFromToken(It.IsAny<string>()), Times.Once());
        _userRepository.Verify(u => 
            u.Get(It.Is<UserId>(id => Utils.User.AssertUserId(id, plainUserId))), 
            Times.Once());
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(user);
    }
    
    [Fact]
    public async Task GetUserFromToken_WhenUserIsNull_ShouldReturnInvalidTokenError()
    {
        // Arrange
        const string plainToken = "This is the user Token";

        var plainUserId = Guid.NewGuid().ToString();
        _jwtTokenGenerator.Setup(j => j.GetUserIdFromToken(plainToken))
            .Returns(plainUserId);
        
        // Act
        var outcome = await _uut.GetUserFromToken(plainToken);

        // Assert
        _jwtTokenGenerator.Verify(j => j.GetUserIdFromToken(It.IsAny<string>()), Times.Once());
        _userRepository.Verify(u => 
                u.Get(It.IsAny<UserId>()), 
            Times.Once());
        
        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Should().Be(Errors.Authentication.InvalidToken);
    }
    
    [Fact]
    public async Task GetUserFromToken_WhenGetUserIdThrowsError_ShouldReturnInvalidTokenError()
    {
        // Arrange
        const string plainToken = "This is the user Token";

        var error = Errors.Authentication.InvalidToken;
        _jwtTokenGenerator.Setup(j => j.GetUserIdFromToken(plainToken))
            .Returns(error);
        
        // Act
        var outcome = await _uut.GetUserFromToken(plainToken);

        // Assert
        _jwtTokenGenerator.Verify(j => j.GetUserIdFromToken(It.IsAny<string>()), Times.Once());
        _userRepository.Verify(u => 
                u.Get(It.IsAny<UserId>()), 
            Times.Never);
        
        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Should().Be(error);
    }
}