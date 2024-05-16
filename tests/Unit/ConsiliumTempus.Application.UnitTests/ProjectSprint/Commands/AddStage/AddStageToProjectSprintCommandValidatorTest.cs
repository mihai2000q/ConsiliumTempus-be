using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.AddStage;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.AddStage;

public class AddStageToProjectSprintCommandValidatorTest
{
    #region Setup

    private readonly AddStageToProjectSprintCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(AddStageToProjectSprintCommandValidatorData.GetValidCommands))]
    public async Task ValidateAddStageToProjectSprintCommand_WhenValid_ShouldReturnTrue(AddStageToProjectSprintCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(AddStageToProjectSprintCommandValidatorData.GetInvalidProjectSprintIdCommands))]
    [ClassData(typeof(AddStageToProjectSprintCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateAddStageToProjectSprintCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        AddStageToProjectSprintCommand command,
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