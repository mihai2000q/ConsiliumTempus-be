using ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.MoveStage;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.MoveStage;

public class MoveStageFromProjectSprintCommandValidatorTest
{
    #region Setup

    private readonly MoveStageFromProjectSprintCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(MoveStageFromProjectSprintCommandValidatorData.GetValidCommands))]
    public async Task ValidateMoveStageFromProjectSprintCommand_WhenValid_ShouldReturnTrue(MoveStageFromProjectSprintCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(MoveStageFromProjectSprintCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(MoveStageFromProjectSprintCommandValidatorData.GetInvalidStageIdCommands))]
    [ClassData(typeof(MoveStageFromProjectSprintCommandValidatorData.GetInvalidOverStageIdCommands))]
    public async Task ValidateMoveStageFromProjectSprintCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        MoveStageFromProjectSprintCommand command,
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