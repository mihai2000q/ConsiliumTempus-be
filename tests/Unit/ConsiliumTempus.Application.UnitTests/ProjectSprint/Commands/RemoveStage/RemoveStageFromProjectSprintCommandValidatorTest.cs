using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.RemoveStage;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.RemoveStage;

public class RemoveStageFromProjectSprintCommandValidatorTest
{
    #region Setup

    private readonly RemoveStageFromProjectSprintCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(RemoveStageFromProjectSprintCommandValidatorData.GetValidCommands))]
    public async Task ValidateRemoveStageFromProjectSprintCommand_WhenValid_ShouldReturnTrue(RemoveStageFromProjectSprintCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(RemoveStageFromProjectSprintCommandValidatorData.GetInvalidProjectSprintIdCommands))]
    [ClassData(typeof(RemoveStageFromProjectSprintCommandValidatorData.GetInvalidProjectSprintIdCommands))]
    public async Task ValidateRemoveStageFromProjectSprintCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        RemoveStageFromProjectSprintCommand command,
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