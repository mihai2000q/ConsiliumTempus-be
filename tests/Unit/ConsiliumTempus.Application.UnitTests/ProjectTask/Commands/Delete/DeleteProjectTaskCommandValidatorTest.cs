using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Delete;

public class DeleteProjectTaskCommandValidatorTest
{
    #region Setup

    private readonly DeleteProjectTaskCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(DeleteProjectTaskCommandValidatorData.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(DeleteProjectTaskCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(DeleteProjectTaskCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(DeleteProjectTaskCommandValidatorData.GetInvalidProjectStageIdCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(
        DeleteProjectTaskCommand command,
        string property,
        int expectedErrors)
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