using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.UpdateOverview;

public class UpdateOverviewWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly UpdateOverviewWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateOverviewWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(UpdateOverviewWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateOverviewWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateOverviewWorkspaceCommand command,
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