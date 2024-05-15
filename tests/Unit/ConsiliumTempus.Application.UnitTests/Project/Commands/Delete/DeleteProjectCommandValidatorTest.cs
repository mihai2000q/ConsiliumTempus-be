using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.Delete;

public class DeleteProjectCommandValidatorTest
{
    #region Setup

    private readonly DeleteProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(DeleteProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateDeleteProjectCommand_WhenValid_ShouldReturnTrue(DeleteProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(DeleteProjectCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateDeleteProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        DeleteProjectCommand command,
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