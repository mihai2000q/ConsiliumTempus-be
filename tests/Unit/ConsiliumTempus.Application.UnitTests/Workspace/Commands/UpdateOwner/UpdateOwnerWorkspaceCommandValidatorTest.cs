using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.UpdateOwner;

public class UpdateOwnerWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly UpdateOwnerWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateOwnerWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateOwnerWorkspaceCommand_WhenValid_ShouldReturnTrue(UpdateOwnerWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateOwnerWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateOwnerWorkspaceCommandValidatorData.GetInvalidOwnerIdCommands))]
    public async Task ValidateUpdateOwnerWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateOwnerWorkspaceCommand command,
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