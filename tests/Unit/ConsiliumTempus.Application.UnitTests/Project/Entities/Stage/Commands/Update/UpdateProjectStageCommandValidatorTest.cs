using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Commands.Update;

public class UpdateProjectStageCommandValidatorTest
{
    #region Setup

    private readonly UpdateProjectStageCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(UpdateProjectStageCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateProjectStageCommand_WhenValid_ShouldReturnTrue(UpdateProjectStageCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(UpdateProjectStageCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateProjectStageCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateUpdateProjectStageCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateProjectStageCommand command, 
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