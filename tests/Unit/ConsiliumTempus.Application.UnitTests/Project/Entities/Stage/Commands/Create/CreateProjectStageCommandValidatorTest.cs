using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Commands.Create;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Commands.Create;

public class CreateProjectStageCommandValidatorTest
{
    #region Setup 

    private readonly CreateProjectStageCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(CreateProjectStageCommandValidatorData.GetValidCommands))]
    public async Task ValidateCreateProjectStageCommand_WhenValid_ShouldReturnTrue(CreateProjectStageCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CreateProjectStageCommandValidatorData.GetInvalidProjectSprintIdCommands))]
    [ClassData(typeof(CreateProjectStageCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateCreateProjectStageCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        CreateProjectStageCommand command,
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