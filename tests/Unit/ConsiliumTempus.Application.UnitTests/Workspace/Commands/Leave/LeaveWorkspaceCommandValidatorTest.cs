using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.Leave;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Leave;

public class LeaveWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly LeaveWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(LeaveWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateLeaveWorkspaceCommand_WhenValid_ShouldReturnTrue(
        LeaveWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(LeaveWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateLeaveWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        LeaveWorkspaceCommand command,
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