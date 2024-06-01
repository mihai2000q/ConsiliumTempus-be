using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetCollection;

public class GetCollectionWorkspaceQueryValidatorTest
{
    #region Setup

    private readonly GetCollectionWorkspaceQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionWorkspaceQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetCollectionWorkspaceQuery_WhenValid_ShouldReturnTrue(GetCollectionWorkspaceQuery query)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(GetCollectionWorkspaceQueryValidatorData.GetInvalidPageSizeAndCurrentPageQueries))]
    [ClassData(typeof(GetCollectionWorkspaceQueryValidatorData.GetInvalidPageSizeQueries))]
    [ClassData(typeof(GetCollectionWorkspaceQueryValidatorData.GetInvalidCurrentPageQueries))]
    [ClassData(typeof(GetCollectionWorkspaceQueryValidatorData.GetInvalidOrderByQueries))]
    [ClassData(typeof(GetCollectionWorkspaceQueryValidatorData.GetInvalidSearchQueries))]
    public async Task ValidateGetCollectionWorkspaceQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollectionWorkspaceQuery query, 
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