using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.InviteCollaborator;

public class InviteCollaboratorToWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly InviteCollaboratorToWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(InviteCollaboratorToWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateInviteCollaboratorToWorkspaceCommand_WhenValid_ShouldReturnTrue(
        InviteCollaboratorToWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(InviteCollaboratorToWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(InviteCollaboratorToWorkspaceCommandValidatorData.GetInvalidEmailCommands))]
    public async Task ValidateInviteCollaboratorToWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        InviteCollaboratorToWorkspaceCommand command,
        string property,
        short expectedErrors)
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