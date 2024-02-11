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
    
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly UpdateUserCommandHandler _uut;
    
    public UpdateUserCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _uut = new UpdateUserCommandHandler(_userRepository.Object, _unitOfWork.Object);
    }

    #endregion

    [Fact]
    public async Task WhenUpdateUserCommandIsSuccessful_ShouldReturnNewUser()
    {
        // Arrange
        var currentUser = Mock.Mock.User.CreateMock();
        _userRepository.Setup(u => u.Get(It.IsAny<UserId>(), default))
            .ReturnsAsync(currentUser);
            
        var command = new UpdateUserCommand(
            currentUser.Id.Value,
            "New First Name",
            "New Last Name");
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _userRepository.Verify(w =>
                w.Get(It.Is<UserId>(id => id.Value == command.Id), 
                    default),
            Times.Once());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once());
        
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
            "New Last Name");
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _userRepository.Verify(w =>
                w.Get(It.Is<UserId>(id => id.Value == command.Id), 
                    default),
            Times.Once());
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never);
        
        outcome.ValidateError(Errors.User.NotFound);
    }
}