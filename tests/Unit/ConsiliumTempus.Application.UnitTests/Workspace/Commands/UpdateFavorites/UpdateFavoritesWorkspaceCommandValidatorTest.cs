using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;
using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.UpdateFavorites;

public class UpdateFavoriteWorkspaceCommandValidatorTest
{
    #region Setup

    private readonly UpdateFavoriteWorkspaceCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateFavoritesWorkspaceCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateFavoritesWorkspaceCommand_WhenValid_ShouldReturnTrue(UpdateFavoritesWorkspaceCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateFavoritesWorkspaceCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateUpdateFavoritesWorkspaceCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateFavoritesWorkspaceCommand command,
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