using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.User.Commands;

public class UpdateUserCommandHandlerTest
{
    #region Setup

    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateUserCommandHandler _uut;

    public UpdateUserCommandHandlerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _uut = new UpdateUserCommandHandler(_userRepository, _unitOfWork);
    }

    #endregion

    [Fact]
    public async Task WhenUpdateUserCommandIsSuccessful_ShouldReturnNewUser()
    {
        // Arrange
        var currentUser = Mock.Mock.User.CreateMock();
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(currentUser);

        var command = new UpdateUserCommand(
            currentUser.Id.Value,
            "New First Name",
            "New Last Name",
            "Footballer",
            null);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => id.Value == command.Id));
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();

        outcome.IsError.Should().BeFalse();
        Utils.User.AssertFromUpdateCommand(outcome.Value, command);
    }

    [Fact]
    public async Task WhenUpdateUserCommandIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "New First Name",
            "New Last Name",
            null,
            null);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => id.Value == command.Id));
        _unitOfWork.DidNotReceive();

        outcome.ValidateError(Errors.User.NotFound);
    }
}