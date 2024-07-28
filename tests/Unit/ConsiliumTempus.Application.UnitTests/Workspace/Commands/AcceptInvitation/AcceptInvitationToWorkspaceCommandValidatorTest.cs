using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.AcceptInvitation;

public class AcceptInvitationToWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly AcceptInvitationToWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(AcceptInvitationToWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateAcceptInvitationToWorkspaceCommand_WhenValid_ShouldReturnTrue(
        AcceptInvitationToWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(AcceptInvitationToWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(AcceptInvitationToWorkspaceCommandValidatorData.GetInvalidInvitationIdCommands))]
    public async Task ValidateAcceptInvitationToWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        AcceptInvitationToWorkspaceCommand command,
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