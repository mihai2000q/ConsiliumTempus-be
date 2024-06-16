using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.Update;

public class UpdateProjectCommandValidatorTest
{
    #region Setup

    private readonly UpdateProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateProjectCommand_WhenValid_ShouldReturnTrue(UpdateProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateProjectCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateProjectCommandValidatorData.GetInvalidNameCommands))]
    [ClassData(typeof(UpdateProjectCommandValidatorData.GetInvalidLifecycleCommands))]
    public async Task ValidateUpdateProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateProjectCommand command,
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