using ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdatePrivacy;

public class UpdatePrivacyProjectCommandValidatorTest
{
    #region Setup

    private readonly UpdatePrivacyProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdatePrivacyProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdatePrivacyProjectCommand_WhenValid_ShouldReturnTrue(UpdatePrivacyProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdatePrivacyProjectCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateUpdatePrivacyProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdatePrivacyProjectCommand command,
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