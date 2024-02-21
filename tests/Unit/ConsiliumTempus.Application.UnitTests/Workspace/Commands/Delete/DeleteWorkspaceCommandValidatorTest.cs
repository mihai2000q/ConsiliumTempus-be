using ConsiliumTempus.Application.Workspace.Commands.Delete;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Delete;

public class DeleteWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly DeleteWorkspaceCommandValidator _uut = new();

    #endregion
    
    [Fact]
    public async Task WhenIsValid_ShouldReturnTrue()
    {
        // Arrange
        var command = new DeleteWorkspaceCommand(Guid.NewGuid());

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
        var command = new DeleteWorkspaceCommand(Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(command.Id)));
    }
}