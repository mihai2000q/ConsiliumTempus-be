using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdateOverview;

public class UpdateOverviewProjectCommandValidatorTest
{
    #region Setup

    private readonly UpdateOverviewProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateOverviewProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateOverviewProjectCommand_WhenValid_ShouldReturnTrue(
        UpdateOverviewProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateOverviewProjectCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateUpdateOverviewProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateOverviewProjectCommand command,
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