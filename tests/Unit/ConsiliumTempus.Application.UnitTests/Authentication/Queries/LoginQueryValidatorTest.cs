using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Application.UnitTests.TestData.Authentication.Queries;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Queries;

public class LoginQueryValidatorTest
{
    #region Setup

    private readonly LoginQueryValidator _uut = new();

    #endregion
    
    [Theory]
    [ClassData(typeof(LoginQueryValidatorData.GetValidQueries))]
    public async Task WhenValid_ShouldReturnTrue(LoginQuery query)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(LoginQueryValidatorData.GetInvalidEmailQueries))]
    [ClassData(typeof(LoginQueryValidatorData.GetInvalidPasswordQueries))]
    public async Task WhenSingleFieldIsInvalid_ShouldReturnFalse(LoginQuery query, string property, int expectedErrors)
    {
        // Arrange - parameterized
        
        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(expectedErrors);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(property));
    }
}