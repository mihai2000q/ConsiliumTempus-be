using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetOverview;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetOverview;

public class GetOverviewProjectQueryValidatorTest
{
    #region Setup

    private readonly GetOverviewProjectQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetOverviewProjectQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetOverviewProjectQuery_WhenQueryIsValid_ShouldReturnTrue(GetOverviewProjectQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetOverviewProjectQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetOverviewProjectOverviewQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetOverviewProjectQuery query,
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