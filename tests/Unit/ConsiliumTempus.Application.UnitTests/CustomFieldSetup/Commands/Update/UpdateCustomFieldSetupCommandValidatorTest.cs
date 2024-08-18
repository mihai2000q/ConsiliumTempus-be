using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Update;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.Update;

public class UpdateCustomFieldSetupCommandValidatorTest
{
    #region Setup

    private readonly UpdateCustomFieldSetupCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateCustomFieldSetupCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateCustomFieldSetupCommand_WhenValid_ShouldReturnTrue(
        UpdateCustomFieldSetupCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateCustomFieldSetupCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateCustomFieldSetupCommandValidatorData.GetInvalidNameCommands))]
    [ClassData(typeof(UpdateCustomFieldSetupCommandValidatorData.GetInvalidTypeCommands))]
    [ClassData(typeof(UpdateCustomFieldSetupCommandValidatorData.GetInvalidNumberCustomFieldSetupCommands))]
    [ClassData(typeof(UpdateCustomFieldSetupCommandValidatorData.GetInvalidSingleSelectCustomFieldSetupCommands))]
    [ClassData(typeof(UpdateCustomFieldSetupCommandValidatorData.GetInvalidTextCustomFieldSetupCommands))]
    public async Task ValidateUpdateCustomFieldSetupCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateCustomFieldSetupCommand command,
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