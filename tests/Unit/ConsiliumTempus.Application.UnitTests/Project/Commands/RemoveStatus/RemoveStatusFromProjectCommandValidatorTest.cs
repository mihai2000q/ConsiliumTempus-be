using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.RemoveStatus;

public class RemoveStatusFromProjectCommandValidatorTest
{
    #region Setup

    private readonly RemoveStatusFromProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(RemoveStatusFromProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateRemoveStatusFromProjectCommand_WhenValid_ShouldReturnTrue(RemoveStatusFromProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(RemoveStatusFromProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(RemoveStatusFromProjectCommandValidatorData.GetInvalidStatusIdCommands))]
    public async Task ValidateRemoveStatusFromProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        RemoveStatusFromProjectCommand command,
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