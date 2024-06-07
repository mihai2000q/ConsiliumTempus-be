using ConsiliumTempus.Application.Project.Queries.GetStatuses;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetStatuses;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetStatuses;

public class GetStatusesFromProjectQueryValidatorTest
{
    #region Setup

    private readonly GetStatusesFromProjectQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetStatusesFromProjectQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetStatusesFromProjectQuery_WhenQueryIsValid_ShouldReturnTrue(GetStatusesFromProjectQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetStatusesFromProjectQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetStatusesFromProjectQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetStatusesFromProjectQuery query,
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