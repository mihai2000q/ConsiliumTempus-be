using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands.Login;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands.Login;

public class LoginCommandValidatorTest
{
    #region Setup

    private readonly LoginCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(LoginCommandValidatorData.GetValidQueries))]
    public async Task WhenValid_ShouldReturnTrue(LoginCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(LoginCommandValidatorData.GetInvalidEmailQueries))]
    [ClassData(typeof(LoginCommandValidatorData.GetInvalidPasswordQueries))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(LoginCommand command, string property, int expectedErrors)
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