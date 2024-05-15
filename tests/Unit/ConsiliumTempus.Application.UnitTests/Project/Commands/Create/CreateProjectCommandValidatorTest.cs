using ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;
using ConsiliumTempus.Application.Project.Commands.Create;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.Create;

public class CreateProjectCommandValidatorTest
{
    #region Setup

    private readonly CreateProjectCommandValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(CreateProjectCommandValidatorData.GetValidCommands))]
    public async Task WhenValid_ShouldReturnTrue(CreateProjectCommand command)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(command);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CreateProjectCommandValidatorData.GetInvalidWorkspaceIdCommands))]
    [ClassData(typeof(CreateProjectCommandValidatorData.GetInvalidNameCommands))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(
        CreateProjectCommand command,
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