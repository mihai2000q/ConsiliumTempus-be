using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetCollection;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetCollection;

public class GetCollectionProjectQueryValidatorTest
{
    #region Setup

    private readonly GetCollectionProjectQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionProjectQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetCollectionProjectQuery_WhenValid_ShouldReturnTrue(GetCollectionProjectQuery query)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(GetCollectionProjectQueryValidatorData.GetInvalidPageSizeQueries))]
    [ClassData(typeof(GetCollectionProjectQueryValidatorData.GetInvalidCurrentPageQueries))]
    [ClassData(typeof(GetCollectionProjectQueryValidatorData.GetInvalidOrderByQueries))]
    [ClassData(typeof(GetCollectionProjectQueryValidatorData.GetInvalidNameQueries))]
    public async Task ValidateGetCollectionProjectQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollectionProjectQuery query, 
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