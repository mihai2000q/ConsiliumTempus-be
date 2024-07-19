using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Delete;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Delete;

public class DeleteProjectTaskCommandValidatorTest
{
    #region Setup

    private readonly DeleteProjectTaskCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(DeleteProjectTaskCommandValidatorData.GetValidCommands))]
    public async Task ValidateDeleteProjectTaskCommand_WhenValid_ShouldReturnTrue(DeleteProjectTaskCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(DeleteProjectTaskCommandValidatorData.GetInvalidStageIdCommands))]
    [ClassData(typeof(DeleteProjectTaskCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateDeleteProjectTaskCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        DeleteProjectTaskCommand command,
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