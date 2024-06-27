using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.Get;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.Get;

public class GetProjectQueryValidatorTest
{
    #region Setup

    private readonly GetProjectQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetProjectQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetProjectQuery_WhenQueryIsValid_ShouldReturnTrue(GetProjectQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }

    [Theory]
    [ClassData(typeof(GetProjectQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetProjectQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetProjectQuery query,
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