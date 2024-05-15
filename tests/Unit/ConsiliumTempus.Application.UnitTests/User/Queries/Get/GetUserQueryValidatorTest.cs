using ConsiliumTempus.Application.UnitTests.TestData.User.Queries;
using ConsiliumTempus.Application.User.Queries.Get;

namespace ConsiliumTempus.Application.UnitTests.User.Queries.Get;

public class GetUserQueryValidatorTest
{
    #region Setup

    private readonly GetUserQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetUserQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetUserQuery_WhenIsValid_ShouldReturnTrue(GetUserQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(GetUserQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetUserQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetUserQuery query,
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