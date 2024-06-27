using ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries.GetStages;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Queries.GetStages;

public class GetStagesFromProjectSprintQueryValidatorTest
{
    #region Setup

    private readonly GetStagesFromProjectSprintQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetStagesFromProjectSprintQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetStagesFromProjectSprintQuery_WhenQueryIsValid_ShouldReturnTrue(
        GetStagesFromProjectSprintQuery query)
    {
        // Arrange - parameterized
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetStagesFromProjectSprintQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetStagesFromProjectSprintQuery_WhenIdIsInvalid_ShouldReturnFalse(
        GetStagesFromProjectSprintQuery query,
        string property)
    {
        // Arrange - parameterized
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(property));
    }
}