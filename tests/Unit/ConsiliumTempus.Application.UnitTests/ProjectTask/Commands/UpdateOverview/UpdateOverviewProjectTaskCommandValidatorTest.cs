using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateOverview;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Commands.UpdateOverview;

public class UpdateOverviewProjectTaskCommandValidatorTest
{
    #region Setup

    private readonly UpdateOverviewProjectTaskCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateOverviewProjectTaskCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateOverviewProjectTaskCommand_WhenValid_ShouldReturnTrue(UpdateOverviewProjectTaskCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateOverviewProjectTaskCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateOverviewProjectTaskCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateUpdateOverviewProjectTaskCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateOverviewProjectTaskCommand command,
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