using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.Get;
using ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Queries.Get;

public class GetProjectSprintQueryValidatorTest
{
    #region Setup

    private readonly GetProjectSprintQueryValidator _uut = new();

    #endregion
    
    [Fact]
    public async Task WhenQueryIsValid_ShouldReturnTrue()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetProjectSprintQuery();

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
        var query = ProjectSprintQueryFactory.CreateGetProjectSprintQuery(Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(query.Id)));
    }
}