using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Create;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Create;

public class CreateProjectTaskCommandValidatorTest
{
    #region Setup

    private readonly CreateProjectTaskCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(CreateProjectTaskCommandValidatorData.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(CreateProjectTaskCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CreateProjectTaskCommandValidatorData.GetInvalidProjectStageIdCommands))]
    [ClassData(typeof(CreateProjectTaskCommandValidatorData.GetInvalidNameCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(
        CreateProjectTaskCommand command,
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