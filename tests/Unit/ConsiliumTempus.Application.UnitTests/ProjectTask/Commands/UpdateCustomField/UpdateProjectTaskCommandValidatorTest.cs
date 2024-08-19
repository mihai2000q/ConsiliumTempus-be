using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateCustomField;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateCustomField;

public class UpdateCustomFieldFromProjectTaskCommandValidatorTest
{
    #region Setup

    private readonly UpdateCustomFieldFromProjectTaskCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateCustomFieldFromProjectTaskCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateCustomFieldFromProjectTaskCommand_WhenValid_ShouldReturnTrue(
        UpdateCustomFieldFromProjectTaskCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateCustomFieldFromProjectTaskCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateCustomFieldFromProjectTaskCommandValidatorData.GetInvalidCustomFieldIdCommands))]
    [ClassData(typeof(UpdateCustomFieldFromProjectTaskCommandValidatorData.GetInvalidTypeCommands))]
    [ClassData(typeof(UpdateCustomFieldFromProjectTaskCommandValidatorData.GetInvalidNumberCustomFieldCommands))]
    [ClassData(typeof(UpdateCustomFieldFromProjectTaskCommandValidatorData.GetInvalidSingleSelectCustomFieldCommands))]
    [ClassData(typeof(UpdateCustomFieldFromProjectTaskCommandValidatorData.GetInvalidTextCustomFieldCommands))]
    public async Task ValidateUpdateCustomFieldFromProjectTaskCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateCustomFieldFromProjectTaskCommand command,
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