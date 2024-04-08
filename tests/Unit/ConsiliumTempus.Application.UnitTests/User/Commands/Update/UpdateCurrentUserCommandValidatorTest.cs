using ConsiliumTempus.Application.UnitTests.TestData.User.Commands;
using ConsiliumTempus.Application.UnitTests.TestData.User.Commands.UpdateCurrent;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;

namespace ConsiliumTempus.Application.UnitTests.User.Commands.Update;

public class UpdateCurrentUserCommandValidatorTest
{
    #region Setup

    private readonly UpdateCurrentUserCommandValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(UpdateCurrentUserCommandValidatorData.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(UpdateCurrentUserCommand command)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(UpdateCurrentUserCommandValidatorData.GetInvalidFirstNameCommands))]
    [ClassData(typeof(UpdateCurrentUserCommandValidatorData.GetInvalidLastNameCommands))]
    [ClassData(typeof(UpdateCurrentUserCommandValidatorData.GetInvalidRoleCommands))]
    [ClassData(typeof(UpdateCurrentUserCommandValidatorData.GetInvalidDateOfBirthCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(UpdateCurrentUserCommand command, string property, int expectedErrors)
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