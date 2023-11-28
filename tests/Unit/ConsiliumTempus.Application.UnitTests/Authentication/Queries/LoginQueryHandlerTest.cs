using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.UserAggregate;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Queries;

public class LoginQueryHandlerTest
{
    #region Setup

    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly LoginQueryHandler _uut;

    public LoginQueryHandlerTest()
    {
        _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _userRepository = new Mock<IUserRepository>();
        _uut = new LoginQueryHandler(_userRepository.Object, _jwtTokenGenerator.Object);
    }

    #endregion

    [Fact]
    public async Task WhenLoginIsSuccessful_ShouldQueryUserAndReturnNewToken()
    {
        // Arrange
        var query = new LoginQuery(
            "Some@Example.com", 
            "Password123");

        var mockUser = new Mock<User>();
        mockUser.SetupGet(u => u.Password).Returns(query.Password);
        _userRepository.Setup(u => u.GetUserByEmail(query.Email))
            .Returns(mockUser.Object);
        
        const string mockToken = "This is a token";
        _jwtTokenGenerator.Setup(j => j.GenerateToken(mockUser.Object))
            .Returns(mockToken);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        _userRepository.Verify(u => u.GetUserByEmail(It.IsAny<string>()), Times.Once());
        _jwtTokenGenerator.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Once());
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(mockToken);
    }
    
    [Fact]
    public async Task WhenLoginFailsDueToMissingUser_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var query = new LoginQuery(
            "Some@Example.com", 
            "Password123");
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        _userRepository.Verify(u => u.GetUserByEmail(It.IsAny<string>()), Times.Once());

        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Type.Should().Be(ErrorType.Conflict);
        outcome.FirstError.Code.Should().Be("Authentication.InvalidCredentials");
        outcome.FirstError.Description.Should().Be("Invalid Credentials");
    }
    
    [Fact]
    public async Task WhenLoginFailsDueToWrongPassword_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var query = new LoginQuery(
            "Some@Example.com", 
            "Password123");
        
        var mockUser = new Mock<User>();
        mockUser.SetupGet(u => u.Password).Returns("Random Password");
        _userRepository.Setup(u => u.GetUserByEmail(query.Email))
            .Returns(mockUser.Object);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        _userRepository.Verify(u => u.GetUserByEmail(It.IsAny<string>()), Times.Once());

        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Type.Should().Be(ErrorType.Conflict);
        outcome.FirstError.Code.Should().Be("Authentication.InvalidCredentials");
        outcome.FirstError.Description.Should().Be("Invalid Credentials");
    }
}