using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdateStatus;

public class UpdateStatusFromProjectCommandValidatorTest
{
    #region Setup

    private readonly UpdateStatusFromProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateStatusFromProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateStatusFromProjectCommand_WhenValid_ShouldReturnTrue(UpdateStatusFromProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateStatusFromProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateStatusFromProjectCommandValidatorData.GetInvalidStatusIdCommands))]
    [ClassData(typeof(UpdateStatusFromProjectCommandValidatorData.GetInvalidTitleCommands))]
    [ClassData(typeof(UpdateStatusFromProjectCommandValidatorData.GetInvalidStatusCommands))]
    [ClassData(typeof(UpdateStatusFromProjectCommandValidatorData.GetInvalidDescriptionCommands))]
    public async Task ValidateUpdateStatusFromProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateStatusFromProjectCommand command,
        string property,
        short expectedErrors)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(expectedErrors);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(property));
    }
}