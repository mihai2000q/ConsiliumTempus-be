using ConsiliumTempus.Application.Project.Commands.UpdateOwner;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdateOwner;

public class UpdateOwnerProjectCommandValidatorTest
{
    #region Setup

    private readonly UpdateOwnerProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateOwnerProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateOwnerProjectCommand_WhenValid_ShouldReturnTrue(UpdateOwnerProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateOwnerProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateOwnerProjectCommandValidatorData.GetInvalidOwnerIdCommands))]
    public async Task ValidateUpdateOwnerProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateOwnerProjectCommand command,
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