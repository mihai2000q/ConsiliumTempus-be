using ConsiliumTempus.Infrastructure.Extensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.Extensions;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("", "")]
    [InlineData("A", "a")]
    [InlineData("AB", "aB")]
    [InlineData("Ab", "ab")]
    [InlineData("ThisIsAVariable", "thisIsAVariable")]
    [InlineData("Someone", "someone")]
    [InlineData("ExpectedValue", "expectedValue")]
    public void FromPascalToCamelCase_LeaveWorkspaceShouldReturnStringWithCamelCaseNotation(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = input.FromPascalToCamelCase();

        // Assert
        outcome.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "Id")]
    [InlineData("role", "roleId")]
    [InlineData("BigRole", "BigRoleId")]
    public void ToId_LeaveWorkspaceShouldReturnStringWithSuffixId(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = input.ToId();

        // Assert
        outcome.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "_")]
    [InlineData("a", "_a")]
    [InlineData("ab", "_ab")]
    [InlineData("aB", "_aB")]
    [InlineData("Role", "_role")]
    [InlineData("WorkspaceRole", "_workspaceRole")]
    public void ToBackingField_LeaveWorkspaceShouldReturnStringWithBackingFieldNotation(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = input.ToBackingField();

        // Assert
        outcome.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "_Id")]
    [InlineData("a", "_aId")]
    [InlineData("ab", "_abId")]
    [InlineData("aB", "_aBId")]
    [InlineData("Role", "_roleId")]
    [InlineData("WorkspaceRole", "_workspaceRoleId")]
    public void ToIdBackingField_LeaveWorkspaceShouldReturnStringWithIdBackingFieldNotation(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = input.ToIdBackingField();

        // Assert
        outcome.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("Workspace", "Workspace")]
    [InlineData("WorkspaceAggreg", "WorkspaceAggreg")]
    [InlineData("WorkspaceAggregate", "Workspace")]
    public void TruncateAggregate_LeaveWorkspaceShouldReturnStringWithoutAggregate(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = input.TruncateAggregate();

        // Assert
        outcome.Should().Be(expected);
    }
}