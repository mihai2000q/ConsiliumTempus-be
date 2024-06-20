using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetCollaborators;

public class GetCollaboratorsFromWorkspaceQueryValidatorTest
{
    #region Setup

    private readonly GetCollaboratorsFromWorkspaceQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(GetCollaboratorsFromWorkspaceQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetCollaboratorsFromWorkspaceQuery_WhenIsValid_ShouldReturnTrue(GetCollaboratorsFromWorkspaceQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetCollaboratorsFromWorkspaceQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetCollaboratorsFromWorkspaceQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollaboratorsFromWorkspaceQuery query,
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