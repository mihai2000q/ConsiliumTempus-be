using ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.UpdateWorkspace;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.MakeGlobal;

public class MakeCustomFieldSetupGlobalCommandValidatorTest
{
    #region Setup

    private readonly MakeCustomFieldSetupGlobalCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(MakeCustomFieldSetupGlobalCommandValidatorData.GetValidCommands))]
    public async Task ValidateMakeCustomFieldSetupGlobalCommand_WhenValid_ShouldReturnTrue(
        MakeCustomFieldSetupGlobalCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(MakeCustomFieldSetupGlobalCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateMakeCustomFieldSetupGlobalCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        MakeCustomFieldSetupGlobalCommand command,
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