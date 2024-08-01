using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.KickCollaborator;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.KickCollaborator;

public class KickCollaboratorFromWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly KickCollaboratorFromWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(KickCollaboratorFromWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateKickCollaboratorWorkspaceCommand_WhenValid_ShouldReturnTrue(
        KickCollaboratorFromWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(KickCollaboratorFromWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(KickCollaboratorFromWorkspaceCommandValidatorData.GetInvalidCollaboratorIdCommands))]
    public async Task ValidateKickCollaboratorWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        KickCollaboratorFromWorkspaceCommand command,
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