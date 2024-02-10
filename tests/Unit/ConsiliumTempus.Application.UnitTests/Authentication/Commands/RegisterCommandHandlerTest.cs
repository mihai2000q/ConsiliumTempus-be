using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands;

public class RegisterCommandHandlerTest
{
    #region Setup

    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;
    private readonly Mock<IScrambler> _scrambler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly RegisterCommandHandler _uut;

    public RegisterCommandHandlerTest()
    {
        _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _scrambler = new Mock<IScrambler>();
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _uut = new RegisterCommandHandler(
            _jwtTokenGenerator.Object,
            _scrambler.Object,
            _userRepository.Object,
            _unitOfWork.Object);
    }

    #endregion

    [Fact]
    public async Task WhenRegisterIsSuccessful_ShouldCreateUserAndReturnNewToken()
    {
        // Arrange
        var command = new RegisterCommand(
            "FirstName",
            "LastName",
            "Example@Email.com",
            "Password123");

        const string token = "This is a token";
        const string hashedPassword = "This is the hash password for Password123";

        UserAggregate? callbackAddedUser = null;
        UserAggregate? callbackUserUsedForJwt = null;
        _userRepository.Setup(r => r.Add(It.IsAny<UserAggregate>()))
            .Callback<UserAggregate>(r => callbackAddedUser = r);
        _jwtTokenGenerator.Setup(j => j.GenerateToken(It.IsAny<UserAggregate>()))
            .Callback<UserAggregate>(r => callbackUserUsedForJwt = r)
            .Returns(token);

        _scrambler.Setup(s => s.HashPassword(command.Password))
            .Returns(hashedPassword);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _userRepository.Verify(r =>
            r.GetUserByEmail(It.IsAny<string>()), Times.Once());
        _userRepository.Verify(r =>
            r.Add(It.IsAny<UserAggregate>()), Times.Once());
        _jwtTokenGenerator.Verify(j =>
            j.GenerateToken(It.IsAny<UserAggregate>()), Times.Once());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once());

        callbackAddedUser.Should().Be(callbackUserUsedForJwt);
        callbackAddedUser?.Id.Should().NotBeNull();
        callbackAddedUser?.Name.First.Should().Be(command.FirstName.CapitalizeWord());
        callbackAddedUser?.Name.Last.Should().Be(command.LastName.CapitalizeWord());
        callbackAddedUser?.Credentials.Email.Should().Be(command.Email.ToLower());
        callbackAddedUser?.Credentials.Password.Should().Be(hashedPassword);
        callbackAddedUser?.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        callbackAddedUser?.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Token.Should().Be(token);
    }

    [Fact]
    public async Task WhenRegisterFindsAnotherUserWithSameEmail_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var command = new RegisterCommand(
            "",
            "",
            "Example@Email.com",
            "");

        _userRepository.Setup(r => r.GetUserByEmail(command.Email.ToLower()))
            .ReturnsAsync(Mock.Mock.User.CreateMock());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _userRepository.Verify(r => r.GetUserByEmail(It.IsAny<string>()), Times.Once());
        _jwtTokenGenerator.Verify(j =>
            j.GenerateToken(It.IsAny<UserAggregate>()), Times.Never());
        _userRepository.Verify(r =>
            r.Add(It.IsAny<UserAggregate>()), Times.Never());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never());

        outcome.ValidateError(Errors.User.DuplicateEmail);
    }
}