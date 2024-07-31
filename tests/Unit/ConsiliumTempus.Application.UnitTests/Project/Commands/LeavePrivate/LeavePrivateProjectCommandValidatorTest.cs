using ConsiliumTempus.Application.Project.Commands.LeavePrivate;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.LeavePrivate;

public class LeavePrivateProjectCommandValidatorTest
{
    #region Setup

    private readonly LeavePrivateProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(LeavePrivateProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateLeavePrivateProjectCommand_WhenValid_ShouldReturnTrue(LeavePrivateProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(LeavePrivateProjectCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateLeavePrivateProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        LeavePrivateProjectCommand command,
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