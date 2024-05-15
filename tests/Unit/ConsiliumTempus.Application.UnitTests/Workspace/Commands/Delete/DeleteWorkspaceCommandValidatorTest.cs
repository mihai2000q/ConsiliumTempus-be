using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Delete;

public class DeleteWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly DeleteWorkspaceCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(DeleteWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateDeleteWorkspaceCommand_WhenValid_ShouldReturnTrue(DeleteWorkspaceCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(DeleteWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateDeleteWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        DeleteWorkspaceCommand command, 
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