using ConsiliumTempus.Application.Common.Interfaces.Persistence;
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

    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly DeleteUserCommandHandler _uut;

    public DeleteUserCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _uut = new DeleteUserCommandHandler(_userRepository.Object, _unitOfWork.Object);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteUserCommandIsSuccessful_ShouldReturnTheUser()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());

        var user = Mock.Mock.User.CreateMock();
        _userRepository.Setup(u => u.Get(It.IsAny<UserId>(), default))
            .ReturnsAsync(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _userRepository.Verify(u =>
                u.Get(It.Is<UserId>(id => id.Value == command.Id), default),
            Times.Once());
        _userRepository.Verify(u => u.Remove(user), Times.Once());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once());

        outcome.IsError.Should().BeFalse();
        Utils.User.AssertDeleteResult(outcome.Value, user);
    }

    [Fact]
    public async Task WhenDeleteUserCommandFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _userRepository.Verify(u =>
                u.Get(It.Is<UserId>(id => id.Value == command.Id), default),
            Times.Once());
        _userRepository.Verify(u => u.Remove(It.IsAny<UserAggregate>()), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never);

        outcome.ValidateError(Errors.User.NotFound);
    }
}