using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.Create;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Create;

public class CreateWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly CreateWorkspaceCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(CreateWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateCreateWorkspaceCommand_WhenValid_ShouldReturnTrue(CreateWorkspaceCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(CreateWorkspaceCommandValidatorData.GetInvalidNameCommands))]
    public async Task ValidateCreateWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        CreateWorkspaceCommand command, 
        string property, 
        int expectedErrors)
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