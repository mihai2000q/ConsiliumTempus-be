using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Create;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.Create;

public class CreateCustomFieldSetupCommandValidatorTest
{
    #region Setup

    private readonly CreateCustomFieldSetupCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetValidCommands))]
    public async Task ValidateCreateCustomFieldSetupCommand_WhenValid_ShouldReturnTrue(
        CreateCustomFieldSetupCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidWorkspaceIdAndProjectIdCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidNameCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidDateCustomFieldSetupCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidDateTimeCustomFieldSetupCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidDurationCustomFieldSetupCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidMultiSelectCustomFieldSetupCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidNumberCustomFieldSetupCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidSingleSelectCustomFieldSetupCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidTextCustomFieldSetupCommands))]
    [ClassData(typeof(CreateCustomFieldSetupCommandValidatorData.GetInvalidTimeCustomFieldSetupCommands))]
    public async Task ValidateCreateCustomFieldSetupCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        CreateCustomFieldSetupCommand command,
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