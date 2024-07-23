using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateIsCompleted;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateIsCompleted;

public class UpdateIsCompletedProjectTaskCommandValidatorTest
{
    #region Setup

    private readonly UpdateIsCompletedProjectTaskCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateIsCompletedProjectTaskCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateIsCompletedProjectTaskCommand_WhenValid_ShouldReturnTrue(UpdateIsCompletedProjectTaskCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateIsCompletedProjectTaskCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateUpdateIsCompletedProjectTaskCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateIsCompletedProjectTaskCommand command,
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