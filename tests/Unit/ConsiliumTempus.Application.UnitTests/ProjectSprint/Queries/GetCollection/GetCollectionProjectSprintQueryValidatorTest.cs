using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Queries.GetCollection;

public class GetCollectionProjectSprintQueryValidatorTest
{
    #region Setup

    private readonly GetCollectionProjectSprintQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetCollectionProjectSprintQueryValidatorData.GetValidQueries))]
    public async Task WhenValid_ShouldReturnTrue(GetCollectionProjectSprintQuery query)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(GetCollectionProjectSprintQueryValidatorData.GetInvalidProjectIdQueries))]
    [ClassData(typeof(GetCollectionProjectSprintQueryValidatorData.GetInvalidSearchQueries))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollectionProjectSprintQuery query, 
        string property,
        short expectedErrors)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(expectedErrors);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(property));
    }
}