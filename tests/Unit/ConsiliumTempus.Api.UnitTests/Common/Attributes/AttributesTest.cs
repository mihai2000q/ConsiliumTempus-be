using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.UnitTests.TestData;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.UnitTests.Common.Attributes;

public class AttributesTest
{
    [Theory]
    [ClassData(typeof(AttributesData.GetPermissions))]
    public void HasPermissionAttribute_WhenCreated_ShouldUseThePermissionNameAsPolicy(Permissions permission)
    {
        // Arrange - parameterized

        // Act
        var outcome = new HasPermissionAttribute(permission);

        // Assert
        outcome.Policy.Should().Be(permission.ToString());
    }

    [Theory]
    [ClassData(typeof(AttributesData.GetWorkspaceAuthorizationLevels))]
    public void HasWorkspaceAuthorizationAttribute_WhenCreated_ShouldUseTheLevelNameAsPolicy(
        WorkspaceAuthorizationLevel level)
    {
        // Arrange - parameterized

        // Act
        var outcome = new HasWorkspaceAuthorizationAttribute(level);

        // Assert
        outcome.Policy.Should().Be(level.ToString());
    }

    [Theory]
    [ClassData(typeof(AttributesData.GetProjectAuthorizationLevels))]
    public void HasProjectAuthorizationAttribute_WhenCreated_ShouldUseTheLevelNameAsPolicy(
        ProjectAuthorizationLevel level)
    {
        // Arrange - parameterized

        // Act
        var outcome = new HasProjectAuthorizationAttribute(level);

        // Assert
        outcome.Policy.Should().Be(level.ToString());
    }
}
