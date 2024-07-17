using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Move;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Move;

public class MoveProjectTaskCommandValidatorTest
{
    #region Setup

    private readonly MoveProjectTaskCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(MoveProjectTaskCommandValidatorData.GetValidCommands))]
    public async Task ValidateMoveProjectTaskCommand_WhenValid_ShouldReturnTrue(MoveProjectTaskCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(MoveProjectTaskCommandValidatorData.GetInvalidSprintIdCommands))]
    [ClassData(typeof(MoveProjectTaskCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(MoveProjectTaskCommandValidatorData.GetInvalidOverIdCommands))]
    public async Task ValidateMoveProjectTaskCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        MoveProjectTaskCommand command,
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