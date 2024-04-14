using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.Get;

public class GetProjectQueryValidatorTest
{
    #region Setup

    private readonly GetProjectQueryValidator _uut = new();

    #endregion
    
    [Fact]
    public async Task WhenQueryIsValid_ShouldReturnTrue()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetProjectQuery();

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
        var query = ProjectQueryFactory.CreateGetProjectQuery(Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(query.Id)));
    }
}