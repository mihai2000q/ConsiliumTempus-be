using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands.Login;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands.Login;

public class LoginCommandValidatorTest
{
    #region Setup

    private readonly LoginCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(LoginCommandValidatorData.GetValidCommands))]
    public async Task ValidateLoginCommand_WhenValid_ShouldReturnTrue(LoginCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(LoginCommandValidatorData.GetInvalidEmailCommands))]
    [ClassData(typeof(LoginCommandValidatorData.GetInvalidPasswordCommands))]
    public async Task ValidateLoginCommand_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        LoginCommand command, 
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