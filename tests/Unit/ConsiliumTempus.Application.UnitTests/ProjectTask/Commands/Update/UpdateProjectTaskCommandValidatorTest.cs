using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Update;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.Update;

public class UpdateProjectTaskCommandValidatorTest
{
    #region Setup

    private readonly UpdateProjectTaskCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateProjectTaskCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateProjectTaskCommand_WhenValid_ShouldReturnTrue(UpdateProjectTaskCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateProjectTaskCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateProjectTaskCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateUpdateProjectTaskCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateProjectTaskCommand command,
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