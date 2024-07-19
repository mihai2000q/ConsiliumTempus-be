using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.Update;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Update;

public class UpdateWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly UpdateWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateWorkspaceCommand_WhenValid_ShouldReturnTrue(UpdateWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateWorkspaceCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateUpdateWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateWorkspaceCommand command,
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