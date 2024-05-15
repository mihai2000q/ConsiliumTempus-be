using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Queries;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Queries.GetCollection;

public class GetCollectionProjectTaskQueryValidatorTest
{
    #region Setup

    private readonly GetCollectionProjectTaskQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionProjectTaskQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetCollectionProjectQuery_WhenValid_ShouldReturnTrue(GetCollectionProjectTaskQuery query)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(GetCollectionProjectTaskQueryValidatorData.GetInvalidProjectStageIdQueries))]
    public async Task ValidateGetCollectionProjectQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollectionProjectTaskQuery query, 
        string property,
        short numberOfErrors)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(numberOfErrors);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(property));
    }
}