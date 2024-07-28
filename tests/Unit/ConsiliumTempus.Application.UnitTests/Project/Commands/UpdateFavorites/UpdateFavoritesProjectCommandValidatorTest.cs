using ConsiliumTempus.Application.Project.Commands.UpdateFavorites;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdateFavorites;

public class UpdateFavoritesProjectCommandValidatorTest
{
    #region Setup

    private readonly UpdateFavoritesProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(UpdateFavoritesProjectCommandValidatorData.GetValidCommands))]
    public async Task ValidateUpdateFavoritesProjectCommand_WhenValid_ShouldReturnTrue(UpdateFavoritesProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(UpdateFavoritesProjectCommandValidatorData.GetInvalidIdCommands))]
    public async Task ValidateUpdateFavoritesProjectCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        UpdateFavoritesProjectCommand command,
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