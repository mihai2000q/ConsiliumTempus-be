using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Delete;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.Delete;

public class DeleteCustomFieldSetupCommandValidatorTest
{
    #region Setup

    private readonly DeleteCustomFieldSetupCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(DeleteCustomFieldSetupCommandValidatorData.GetValidCommands))]
    public async Task ValidateDeleteCustomFieldSetupCommand_WhenValid_ShouldReturnTrue(
        DeleteCustomFieldSetupCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(DeleteCustomFieldSetupCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateDeleteCustomFieldSetupCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        DeleteCustomFieldSetupCommand command,
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