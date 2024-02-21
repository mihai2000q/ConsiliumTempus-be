using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Commands.Delete;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.User.Commands;

public class DeleteUserCommandHandlerTest
{
    #region Setup

    private readonly IUserRepository _userRepository;
    private readonly DeleteUserCommandHandler _uut;

    public DeleteUserCommandHandlerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new DeleteUserCommandHandler(_userRepository);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteUserCommandIsSuccessful_ShouldDeleteAndReturnDeleteResult()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());

        var user = Mock.Mock.User.CreateMock();
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => id.Value == command.Id));
        _userRepository
            .Received(1)
            .Remove(user);
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteUserResult());
    }

    [Fact]
    public async Task WhenDeleteUserCommandFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => id.Value == command.Id));
        _userRepository
            .DidNotReceive()
            .Remove(Arg.Any<UserAggregate>());

        outcome.ValidateError(Errors.User.NotFound);
    }
}