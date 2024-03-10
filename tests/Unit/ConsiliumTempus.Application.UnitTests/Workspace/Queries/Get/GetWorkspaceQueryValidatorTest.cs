using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.Get;

public class GetWorkspaceQueryValidatorTest
{
    #region Setup

    private readonly GetWorkspaceQueryValidator _uut = new();

    #endregion
    
    [Fact]
    public async Task WhenIsValid_ShouldReturnTrue()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetWorkspaceQuery(id: Guid.NewGuid());

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Fact]
    public async Task WhenIdIsInvalid_ShouldReturnFalse()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetWorkspaceQuery(id: Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(query.Id)));
    }
}