using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.AddStatus;

public class AddStatusToProjectCommandValidatorTest
{
    #region Setup

    private readonly AddStatusToProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(AddStatusToProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateAddStatusToProjectCommand_WhenValid_ShouldReturnTrue(AddStatusToProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(AddStatusToProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(AddStatusToProjectCommandValidatorData.GetInvalidTitleCommands))]
    [ClassData(typeof(AddStatusToProjectCommandValidatorData.GetInvalidStatusCommands))]
    [ClassData(typeof(AddStatusToProjectCommandValidatorData.GetInvalidDescriptionCommands))]
    public async Task ValidateAddStatusToProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        AddStatusToProjectCommand command,
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