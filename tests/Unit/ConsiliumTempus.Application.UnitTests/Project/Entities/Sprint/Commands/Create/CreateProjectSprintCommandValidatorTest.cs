using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Sprint.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Commands.Create;

public class CreateProjectSprintCommandValidatorTest
{
    #region Setup

    private readonly CreateProjectSprintCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(CreateProjectSprintCommandValidatorData.GetValidCommands))]
    public async Task ValidateCreateProjectSprintCommand_WhenValid_ShouldReturnTrue(CreateProjectSprintCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CreateProjectSprintCommandValidatorData.GetInvalidProjectIdCommands))]
    [ClassData(typeof(CreateProjectSprintCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateCreateProjectSprintCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        CreateProjectSprintCommand command,
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