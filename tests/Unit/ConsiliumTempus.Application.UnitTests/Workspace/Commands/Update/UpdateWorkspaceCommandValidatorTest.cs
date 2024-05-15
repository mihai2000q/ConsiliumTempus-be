using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Update;

public class UpdateWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly UpdateWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(UpdateWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateWorkspaceCommandValidatorData.GetInvalidNameCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateWorkspaceCommand command,
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

    [Fact]
    public async Task WhenIdIsInvalid_ShouldReturnFalse()
    {
        // Arrange
        var command = WorkspaceCommandFactory.CreateUpdateWorkspaceCommand(id: Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(command.Id)));
    }
}