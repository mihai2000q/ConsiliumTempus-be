using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Sprint.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Commands.Delete;

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