using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Common.UnitTests.User;

namespace ConsiliumTempus.Application.UnitTests.User.Queries.Get;

public class GetUserQueryValidatorTest
{
    #region Setup

    private readonly GetUserQueryValidator _uut = new();

    #endregion

    [Fact]
    public async Task WhenIsValid_ShouldReturnTrue()
    {
        // Arrange
        var query = UserQueryFactory.CreateGetUserQuery(id: Guid.NewGuid());

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
        var query = UserQueryFactory.CreateGetUserQuery(id: Guid.Empty);

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(1);
        outcome.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be(nameof(query.Id)));
    }
}