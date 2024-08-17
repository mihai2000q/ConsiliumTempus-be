using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Queries;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Queries.Get;

public class GetCustomFieldSetupQueryValidatorTest
{
    #region Setup

    private readonly GetCustomFieldSetupQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetCustomFieldSetupQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetCustomFieldSetupQuery_WhenValid_ShouldReturnTrue(
        GetCustomFieldSetupQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(GetCustomFieldSetupQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetCustomFieldSetupQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetCustomFieldSetupQuery query,
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