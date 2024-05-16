using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.Delete;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.Delete;

public class DeleteProjectSprintCommandValidatorTest
{
    #region Setup

    private readonly DeleteProjectSprintCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(DeleteProjectSprintCommandValidatorData.GetValidCommands))]
    public async Task ValidateDeleteProjectSprintCommand_WhenValid_ShouldReturnTrue(DeleteProjectSprintCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(DeleteProjectSprintCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateDeleteProjectSprintCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        DeleteProjectSprintCommand command,
        string property)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(property));
    }
}