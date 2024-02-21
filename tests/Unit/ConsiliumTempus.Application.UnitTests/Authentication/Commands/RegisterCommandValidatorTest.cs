using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Commands;

public class RegisterCommandValidatorTest
{
    #region Setup

    private readonly RegisterCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(Data.RegisterCommandValidator.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(RegisterCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(Data.RegisterCommandValidator.GetInvalidFirstNameCommands))]
    [ClassData(typeof(Data.RegisterCommandValidator.GetInvalidLastNameCommands))]
    [ClassData(typeof(Data.RegisterCommandValidator.GetInvalidEmailCommands))]
    [ClassData(typeof(Data.RegisterCommandValidator.GetInvalidPasswordCommands))]
    [ClassData(typeof(Data.RegisterCommandValidator.GetInvalidRoleCommands))]
    [ClassData(typeof(Data.RegisterCommandValidator.GetInvalidDateOfBirthCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(RegisterCommand command, string property, int expectedErrors)
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