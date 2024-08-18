using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.RemoveFromProject;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.RemoveFromProject;

public class RemoveCustomFieldSetupFromProjectCommandValidatorTest
{
    #region Setup

    private readonly RemoveCustomFieldSetupFromProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(RemoveCustomFieldSetupFromProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateRemoveCustomFieldSetupFromProjectCommand_WhenValid_ShouldReturnTrue(
        RemoveCustomFieldSetupFromProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(RemoveCustomFieldSetupFromProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(RemoveCustomFieldSetupFromProjectCommandValidatorData.GetInvalidProjectIdCommands))]
    public async Task ValidateRemoveCustomFieldSetupFromProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        RemoveCustomFieldSetupFromProjectCommand command,
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