using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Sprint.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Commands.Update;

public class UpdateProjectSprintCommandValidatorTest
{
    #region Setup

    private readonly UpdateProjectSprintCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(UpdateProjectSprintCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateProjectSprintCommand_WhenValid_ShouldReturnTrue(UpdateProjectSprintCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(UpdateProjectSprintCommandValidatorData.GetInvalidIdCommands))]
    [ClassData(typeof(UpdateProjectSprintCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateUpdateProjectSprintCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateProjectSprintCommand command, 
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