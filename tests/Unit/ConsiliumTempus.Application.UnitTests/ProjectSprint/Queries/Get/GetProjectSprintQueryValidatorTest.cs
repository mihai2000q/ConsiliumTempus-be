using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries.Get;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Queries.Get;

public class GetProjectSprintQueryValidatorTest
{
    #region Setup

    private readonly GetProjectSprintQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetProjectSprintQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetProjectSprintQuery_WhenQueryIsValid_ShouldReturnTrue(GetProjectSprintQuery query)
    {
        // Arrange - parameterized
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetProjectSprintQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetProjectSprintQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse( 
        GetProjectSprintQuery query,
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