using ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.RemoveAllowedMember;

public class RemoveAllowedMemberFromProjectCommandValidatorTest
{
    #region Setup

    private readonly RemoveAllowedMemberFromProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(RemoveAllowedMemberFromProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateRemoveAllowedMemberFromProjectCommand_WhenValid_ShouldReturnTrue(RemoveAllowedMemberFromProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(RemoveAllowedMemberFromProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(RemoveAllowedMemberFromProjectCommandValidatorData.GetInvalidAllowedMemberIdIdCommands))]
    public async Task ValidateRemoveAllowedMemberFromProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        RemoveAllowedMemberFromProjectCommand command,
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