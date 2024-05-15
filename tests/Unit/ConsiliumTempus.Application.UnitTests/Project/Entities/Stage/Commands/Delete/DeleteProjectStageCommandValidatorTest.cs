using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Commands.Delete;

public class DeleteProjectStageCommandValidatorTest
{
    #region Setup 

    private readonly DeleteProjectStageCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(DeleteProjectStageCommandValidatorData.GetValidCommands))]
    public async Task ValidateDeleteProjectStageCommand_WhenValid_ShouldReturnTrue(DeleteProjectStageCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(DeleteProjectStageCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateDeleteProjectStageCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        DeleteProjectStageCommand command,
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