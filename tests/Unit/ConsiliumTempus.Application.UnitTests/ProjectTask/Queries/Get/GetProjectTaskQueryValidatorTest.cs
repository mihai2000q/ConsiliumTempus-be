using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Queries;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Queries.Get;

public class GetProjectTaskQueryValidatorTest
{
    #region Setup

    private readonly GetProjectTaskQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetProjectTaskQueryValidatorData.GetValidQueries))]
    public async Task HandleGetProjectTaskQuery_WhenQueryIsValid_ShouldReturnTrue(GetProjectTaskQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetProjectTaskQueryValidatorData.GetInvalidIdQueries))]
    public async Task HandleGetProjectTaskQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetProjectTaskQuery query,
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