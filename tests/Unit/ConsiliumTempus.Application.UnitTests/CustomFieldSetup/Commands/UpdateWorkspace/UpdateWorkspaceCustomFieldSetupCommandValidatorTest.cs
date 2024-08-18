using ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.UpdateWorkspace;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.UpdateWorkspace;

public class UpdateWorkspaceCustomFieldSetupCommandValidatorTest
{
    #region Setup

    private readonly UpdateWorkspaceCustomFieldSetupCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateWorkspaceFromCustomFieldSetupCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateWorkspaceFromCustomFieldSetupCommand_WhenValid_ShouldReturnTrue(
        UpdateWorkspaceCustomFieldSetupCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateWorkspaceFromCustomFieldSetupCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateWorkspaceFromCustomFieldSetupCommandValidatorData.GetInvalidWorkspaceIdCommands))]
    public async Task ValidateUpdateWorkspaceFromCustomFieldSetupCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateWorkspaceCustomFieldSetupCommand command,
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