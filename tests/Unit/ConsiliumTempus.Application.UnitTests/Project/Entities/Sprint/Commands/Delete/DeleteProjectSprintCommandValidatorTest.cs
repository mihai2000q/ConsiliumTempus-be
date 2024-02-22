using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Commands.Delete;

public class DeleteProjectSprintCommandValidatorTest
{
    #region Setup

    private readonly DeleteProjectSprintCommandValidator _uut = new();

    #endregion

    [Fact]
    public async Task WhenIsValid_ShouldReturnTrue()
    {
        // Arrange
        var command = new DeleteProjectSprintCommand(Guid.NewGuid());

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
        var command = new DeleteProjectSprintCommand(Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(command.Id)));
    }
}