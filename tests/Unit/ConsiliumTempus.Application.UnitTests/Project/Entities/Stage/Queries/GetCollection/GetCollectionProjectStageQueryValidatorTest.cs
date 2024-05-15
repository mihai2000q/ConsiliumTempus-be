using ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Queries;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Queries.GetCollection;

public class GetCollectionProjectStageQueryValidatorTest
{
    #region Setup 

    private readonly GetCollectionProjectStageQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionProjectStageQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetCollectionProjectStageQuery_WhenValid_ShouldReturnTrue(GetCollectionProjectStageQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(GetCollectionProjectStageQueryValidatorData.GetInvalidProjectSprintIdQueries))]
    public async Task ValidateGetCollectionProjectStageQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollectionProjectStageQuery query,
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