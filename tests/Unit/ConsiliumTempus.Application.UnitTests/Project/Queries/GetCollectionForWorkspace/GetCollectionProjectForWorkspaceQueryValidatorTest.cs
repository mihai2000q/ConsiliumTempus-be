using ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetCollectionForWorkspace;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetCollectionForWorkspace;

public class GetCollectionProjectForWorkspaceQueryValidatorTest
{
    #region Setup

    private readonly GetCollectionProjectForWorkspaceQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionProjectForWorkspaceQueryValidatorData.GetValidQueries))]
    public async Task WhenValid_ShouldReturnTrue(GetCollectionProjectForWorkspaceQuery query)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(GetCollectionProjectForWorkspaceQueryValidatorData.GetInvalidWorkspaceIdQueries))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollectionProjectForWorkspaceQuery query, 
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