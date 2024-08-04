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
    [ClassData(typeof(GetCollaboratorsFromWorkspaceQueryValidatorData.GetInvalidPageSizeAndCurrentPageQueries))]
    [ClassData(typeof(GetCollaboratorsFromWorkspaceQueryValidatorData.GetInvalidPageSizeQueries))]
    [ClassData(typeof(GetCollaboratorsFromWorkspaceQueryValidatorData.GetInvalidCurrentPageQueries))]
    [ClassData(typeof(GetCollaboratorsFromWorkspaceQueryValidatorData.GetInvalidOrderByQueries))]
    [ClassData(typeof(GetCollaboratorsFromWorkspaceQueryValidatorData.GetInvalidSearchQueries))]
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