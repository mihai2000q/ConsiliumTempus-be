using ConsiliumTempus.Application.Project.Commands.AddAllowedMember;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.AddAllowedMember;

public class AddAllowedMemberToProjectCommandValidatorTest
{
    #region Setup

    private readonly AddAllowedMemberToProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(AddAllowedMemberToProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateAddAllowedMemberToProjectCommand_WhenValid_ShouldReturnTrue(AddAllowedMemberToProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(AddAllowedMemberToProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(AddAllowedMemberToProjectCommandValidatorData.GetInvalidCollaboratorIdIdCommands))]
    public async Task ValidateAddAllowedMemberToProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        AddAllowedMemberToProjectCommand command,
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