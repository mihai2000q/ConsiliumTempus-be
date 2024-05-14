using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Queries.Get;

public class GetProjectTaskQueryValidatorTest
{
    #region Setup

    private readonly GetProjectTaskQueryValidator _uut = new();

    #endregion
    
    [Fact]
    public async Task WhenQueryIsValid_ShouldReturnTrue()
    {
        // Arrange
        var query = ProjectTaskQueryFactory.CreateGetProjectTaskQuery();

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
        var query = ProjectTaskQueryFactory.CreateGetProjectTaskQuery(Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(query.Id)));
    }
}