using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Queries;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Queries.GetCollection;

public class GetCollectionCustomFieldSetupQueryValidatorTest
{
    #region Setup

    private readonly GetCollectionCustomFieldSetupQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionCustomFieldSetupQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetCollectionCustomFieldSetupQuery_WhenValid_ShouldReturnTrue(
        GetCollectionCustomFieldSetupQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(GetCollectionCustomFieldSetupQueryValidatorData.GetInvalidWorkspaceIdAndProjectIdQueries))]
    public async Task ValidateGetCollectionCustomFieldSetupQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCollectionCustomFieldSetupQuery query,
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