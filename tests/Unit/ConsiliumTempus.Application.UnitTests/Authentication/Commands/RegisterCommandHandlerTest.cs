using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.UserAggregate;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands;

public class RegisterCommandHandlerTest
{
    #region Setup

    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;
    private readonly Mock<IScrambler> _scrambler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly RegisterCommandHandler _uut;

    public RegisterCommandHandlerTest()
    {
        _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _scrambler = new Mock<IScrambler>();
        _userRepository = new Mock<IUserRepository>();
        _uut = new RegisterCommandHandler(_jwtTokenGenerator.Object, _scrambler.Object, _userRepository.Object);
    }

    #endregion

    [Fact]
    public async Task WhenRegisterIsSuccessful_ShouldCreateUserAndReturnNewToken()
    {
        // Arrange
        const string password = "Password123";
        var command = new RegisterCommand(
            "FirstName",
            "LastName",
            "Example@Email.com",
            password);

        const string token = "This is a token";
        const string hashedPassword = "This is the hash password for Password123";

        User? callbackAddedUser = null;
        User? callbackUserUsedForJwt = null;
        _userRepository.Setup(r => r.Add(It.IsAny<User>()))
            .Callback<User>(r => callbackAddedUser = r);
        _jwtTokenGenerator.Setup(j => j.GenerateToken(It.IsAny<User>()))
            .Callback<User>(r => callbackUserUsedForJwt = r)
            .Returns(token);

        _scrambler.Setup(s => s.HashPassword(password))
            .Returns(hashedPassword);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _userRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once());
        _jwtTokenGenerator.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Once());
        
        callbackAddedUser.Should().Be(callbackUserUsedForJwt);
        callbackAddedUser?.Id.Should().NotBeNull();
        callbackAddedUser?.FirstName.Should().Be(command.FirstName);
        callbackAddedUser?.LastName.Should().Be(command.LastName);
        callbackAddedUser?.Email.Should().Be(command.Email);
        callbackAddedUser?.Password.Should().Be(hashedPassword);
        callbackAddedUser?.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        callbackAddedUser?.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);
    }

    [Fact]
    public async Task WhenRegisterFindsAnotherUserWithSameEmail_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        const string email = "Example@Email.com";
        var command = new RegisterCommand(
            "",
            "",
            email,
            "");

        _userRepository.Setup(r => r.GetUserByEmail(email))
            .Returns(new Mock<User>().Object);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _userRepository.Verify(r => r.GetUserByEmail(email), Times.Once());
        
        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Type.Should().Be(ErrorType.Conflict);
        outcome.FirstError.Code.Should().Be("User.DuplicateEmail");
        outcome.FirstError.Description.Should().Be("Email is already in use");
    }
}