using ConsiliumTempus.Application.UnitTests.TestData.User.Commands;
using ConsiliumTempus.Application.User.Commands.Update;

namespace ConsiliumTempus.Application.UnitTests.User.Commands.Update;

public class UpdateUserCommandValidatorTest
{
    #region Setup

    private readonly UpdateUserCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(UpdateUserCommandValidatorData.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(UpdateUserCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(UpdateUserCommandValidatorData.GetInvalidFirstNameCommands))]
    [ClassData(typeof(UpdateUserCommandValidatorData.GetInvalidLastNameCommands))]
    [ClassData(typeof(UpdateUserCommandValidatorData.GetInvalidRoleCommands))]
    [ClassData(typeof(UpdateUserCommandValidatorData.GetInvalidDateOfBirthCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(UpdateUserCommand command, string property, int expectedErrors)
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