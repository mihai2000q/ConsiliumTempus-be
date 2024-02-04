using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.UserAggregate;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Queries;

public class LoginQueryHandlerTest
{
    #region Setup

    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;
    private readonly Mock<IScrambler> _scrambler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly LoginQueryHandler _uut;

    public LoginQueryHandlerTest()
    {
        _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _scrambler = new Mock<IScrambler>();
        _userRepository = new Mock<IUserRepository>();
        _uut = new LoginQueryHandler(_userRepository.Object, _scrambler.Object, _jwtTokenGenerator.Object);
    }

    #endregion

    [Fact]
    public async Task WhenLoginIsSuccessful_ShouldQueryUserAndReturnNewToken()
    {
        // Arrange
        var query = new LoginQuery(
            "Some@Example.com", 
            "Password123");

        const string hashedPassword = "This is the has for Password123";
        
        var user = Mock.Mock.User.CreateMock(password: hashedPassword);
        _userRepository.Setup(u => u.GetUserByEmail(query.Email))
            .ReturnsAsync(user);

        _scrambler.Setup(s => s.VerifyPassword(query.Password, hashedPassword))
            .Returns(true);
        
        const string mockToken = "This is a token";
        _jwtTokenGenerator.Setup(j => j.GenerateToken(user))
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
        _userRepository.Verify(u => u.GetUserByEmail(query.Email), Times.Once());

        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Type.Should().Be(ErrorType.Unauthorized);
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

        var user = Mock.Mock.User.CreateMock(email: query.Email);
        _userRepository.Setup(u => u.GetUserByEmail(query.Email))
            .ReturnsAsync(user);
        
        _scrambler.Setup(s => s.VerifyPassword(query.Password, It.IsAny<string>()))
            .Returns(false);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        _userRepository.Verify(u => u.GetUserByEmail(It.IsAny<string>()), Times.Once());

        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        outcome.FirstError.Code.Should().Be("Authentication.InvalidCredentials");
        outcome.FirstError.Description.Should().Be("Invalid Credentials");
    }
}