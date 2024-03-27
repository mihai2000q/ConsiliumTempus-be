using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands.Refresh;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands.Refresh;

public class RefreshCommandValidatorTest
{
    #region Setup

    private readonly RefreshCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(RefreshCommandValidatorData.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(RefreshCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(RefreshCommandValidatorData.GetInvalidTokenCommands))]
    [ClassData(typeof(RefreshCommandValidatorData.GetInvalidRefreshTokenCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(RefreshCommand command, string property)
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