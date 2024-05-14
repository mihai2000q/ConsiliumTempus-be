using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetOverview;

public class GetOverviewProjectQueryValidatorTest
{
    #region Setup

    private readonly GetOverviewProjectQueryValidator _uut = new();

    #endregion
    
    [Fact]
    public async Task WhenQueryIsValid_ShouldReturnTrue()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetOverviewProjectQuery();

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
        var query = ProjectQueryFactory.CreateGetOverviewProjectQuery(Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(query.Id)));
    }
}