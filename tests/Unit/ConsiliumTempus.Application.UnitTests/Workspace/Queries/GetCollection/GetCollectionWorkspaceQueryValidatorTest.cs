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
    public async Task WhenValid_ShouldReturnTrue(GetCollectionWorkspaceQuery query)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(GetCollectionWorkspaceQueryValidatorData.GetInvalidOrderQueries))]
    [ClassData(typeof(GetCollectionWorkspaceQueryValidatorData.GetInvalidNameQueries))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollectionWorkspaceQuery query, 
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