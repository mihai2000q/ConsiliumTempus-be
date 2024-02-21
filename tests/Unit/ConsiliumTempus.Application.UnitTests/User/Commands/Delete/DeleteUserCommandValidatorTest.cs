using ConsiliumTempus.Application.User.Commands.Delete;

namespace ConsiliumTempus.Application.UnitTests.User.Commands.Delete;

public class DeleteUserCommandValidatorTest
{
    #region Setup

    private readonly DeleteUserCommandValidator _uut = new();

    #endregion

    [Fact]
    public async Task WhenIsValid_ShouldReturnTrue()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Fact]
    public async Task WhenIdIsInvalid_ShouldReturnFalse()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.Empty);
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(command.Id)));
    }
}