using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.AddToProject;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.AddToProject;

public class AddCustomFieldSetupToProjectCommandValidatorTest
{
    #region Setup

    private readonly AddCustomFieldSetupToProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(AddCustomFieldSetupToProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateAddCustomFieldSetupToProjectCommand_WhenValid_ShouldReturnTrue(
        AddCustomFieldSetupToProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(AddCustomFieldSetupToProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(AddCustomFieldSetupToProjectCommandValidatorData.GetInvalidProjectIdCommands))]
    public async Task ValidateAddCustomFieldSetupToProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        AddCustomFieldSetupToProjectCommand command,
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