using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.Get;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.Get;

public class GetWorkspaceQueryValidatorTest
{
    #region Setup

    private readonly GetWorkspaceQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetWorkspaceQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetWorkspaceQuery_WhenIsValid_ShouldReturnTrue(GetWorkspaceQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetWorkspaceQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetWorkspaceQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetWorkspaceQuery query,
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