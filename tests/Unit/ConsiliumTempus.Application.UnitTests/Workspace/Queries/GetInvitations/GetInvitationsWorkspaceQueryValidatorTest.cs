using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetInvitations;

public class GetInvitationsWorkspaceQueryValidatorTest
{
    #region Setup

    private readonly GetInvitationsWorkspaceQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetInvitationsWorkspaceQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetInvitationsWorkspaceQuery_WhenIsValid_ShouldReturnTrue(GetInvitationsWorkspaceQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }

    [Theory]
    [ClassData(typeof(GetInvitationsWorkspaceQueryValidatorData.GetInvalidIsSenderAndWorkspaceIdQueries))]
    [ClassData(typeof(GetInvitationsWorkspaceQueryValidatorData.GetInvalidPageSizeAndCurrentPageQueries))]
    [ClassData(typeof(GetInvitationsWorkspaceQueryValidatorData.GetInvalidPageSizeQueries))]
    [ClassData(typeof(GetInvitationsWorkspaceQueryValidatorData.GetInvalidCurrentPageQueries))]
    public async Task ValidateGetInvitationsWorkspaceQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetInvitationsWorkspaceQuery query,
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