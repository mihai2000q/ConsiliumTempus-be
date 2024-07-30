using ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;
using ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetAllowedMembers;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetAllowedMembers;

public class GetAllowedMembersAllowedMembersFromProjectQueryValidatorTest
{
    #region Setup

    private readonly GetAllowedMembersFromProjectQueryValidator _uut = new();

    #endregion

    [Theory]
    [ClassData(typeof(GetAllowedMembersFromProjectQueryValidatorData.GetValidQueries))]
    public async Task ValidateGetAllowedMembersFromProjectQuery_WhenQueryIsValid_ShouldReturnTrue(
        GetAllowedMembersFromProjectQuery query)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(query);

        // Assert
        outcome.IsValid.Should().BeTrue();
        outcome.Errors.Should().BeEmpty();
    }

    [Theory]
    [ClassData(typeof(GetAllowedMembersFromProjectQueryValidatorData.GetInvalidIdQueries))]
    public async Task ValidateGetAllowedMembersFromProjectQuery_WhenSingleFieldIsInvalid_ShouldReturnFalse(
        GetAllowedMembersFromProjectQuery query,
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