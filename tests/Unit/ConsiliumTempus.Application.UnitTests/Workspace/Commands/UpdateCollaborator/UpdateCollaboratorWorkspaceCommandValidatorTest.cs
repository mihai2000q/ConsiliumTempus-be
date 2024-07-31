using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.UpdateCollaborator;

public class UpdateCollaboratorFromWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly UpdateCollaboratorFromWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateCollaboratorFromWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateCollaboratorWorkspaceCommand_WhenValid_ShouldReturnTrue(
        UpdateCollaboratorFromWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateCollaboratorFromWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateCollaboratorFromWorkspaceCommandValidatorData.GetInvalidCollaboratorIdCommands))]
    [ClassData(typeof(UpdateCollaboratorFromWorkspaceCommandValidatorData.GetInvalidWorkspaceRoleCommands))]
    public async Task ValidateUpdateCollaboratorWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateCollaboratorFromWorkspaceCommand command,
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