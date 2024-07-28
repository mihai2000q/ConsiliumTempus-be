using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.RejectInvitation;

public class RejectInvitationToWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly RejectInvitationToWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(RejectInvitationToWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateRejectInvitationToWorkspaceCommand_WhenValid_ShouldReturnTrue(
        RejectInvitationToWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(RejectInvitationToWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(RejectInvitationToWorkspaceCommandValidatorData.GetInvalidInvitationIdCommands))]
    public async Task ValidateRejectInvitationToWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        RejectInvitationToWorkspaceCommand command,
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