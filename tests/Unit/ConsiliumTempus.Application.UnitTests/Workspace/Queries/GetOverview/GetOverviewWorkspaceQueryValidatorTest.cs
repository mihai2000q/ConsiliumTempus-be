using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetOverview;
using ConsiliumTempus.Application.Workspace.Queries.GetOverview;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetOverview;

public class GetOverviewWorkspaceQueryValidatorTest
{
    #region Setup

    private readonly GetOverviewWorkspaceQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetOverviewWorkspaceQueryValidatorData.GetOverviewValidQueries))]
    public async Task ValidateGetOverviewWorkspaceQuery_WhenIsValid_ShouldReturnTrue(GetOverviewWorkspaceQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetOverviewWorkspaceQueryValidatorData.GetOverviewInvalidIdQueries))]
    public async Task ValidateGetOverviewWorkspaceQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetOverviewWorkspaceQuery query,
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