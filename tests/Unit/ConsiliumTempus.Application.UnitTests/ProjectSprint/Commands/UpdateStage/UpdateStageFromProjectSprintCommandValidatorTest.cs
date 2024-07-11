using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.UpdateStage;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.UpdateStage;

public class UpdateStageFromProjectSprintCommandValidatorTest
{
    #region Setup

    private readonly UpdateStageFromProjectSprintCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateStageFromProjectSprintCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateStageFromProjectSprintCommand_WhenValid_ShouldReturnTrue(UpdateStageFromProjectSprintCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateStageFromProjectSprintCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateStageFromProjectSprintCommandValidatorData.GetInvalidStageIdCommands))]
    [ClassData(typeof(UpdateStageFromProjectSprintCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateUpdateStageFromProjectSprintCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateStageFromProjectSprintCommand command,
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