using ConsiliumTempus.Application.Common.Extensions;

namespace ConsiliumTempus.Application.UnitTests.Common.Extensions;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("first", "second", "first&second")]
    public void And_ShouldConcatenate2StringsWithAndSymbol(string input1, string input2, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = input1.And(input2);

        // Assert
        outcome.Should().Be(expected);
    }

    [Theory]
    [InlineData("first", "second", "first.second")]
    public void Dot_ShouldConcatenate2StringsWithDotSymbol(string input1, string input2, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = input1.Dot(input2);

        // Assert
        outcome.Should().Be(expected);
    }

    [Fact]
    public void ContainsUppercase_WhenStringContainsUppercaseLetter_ShouldReturnTrue()
    {
        // Arrange
        const string str = "Uppercase";

        // Act
        var outcome = str.ContainsUppercase();

        // Assert
        outcome.Should().BeTrue();
    }

    [Fact]
    public void ContainsUppercase_WhenStringDoesNotContainUppercaseLetter_ShouldReturnFalse()
    {
        // Arrange
        const string str = "uppercase";

        // Act
        var outcome = str.ContainsUppercase();

        // Assert
        outcome.Should().BeFalse();
    }

    [Fact]
    public void ContainsLowercase_WhenStringContainsLowercaseLetter_ShouldReturnTrue()
    {
        // Arrange
        const string str = "lowercase";

        // Act
        var outcome = str.ContainsLowercase();

        // Assert
        outcome.Should().BeTrue();
    }

    [Fact]
    public void ContainsLowercase_WhenStringDoesNotContainLowercaseLetter_ShouldReturnFalse()
    {
        // Arrange
        const string str = "LOWERCASE";

        // Act
        var outcome = str.ContainsLowercase();

        // Assert
        outcome.Should().BeFalse();
    }

    [Fact]
    public void ContainsNumber_WhenStringContainsNumber_ShouldReturnTrue()
    {
        // Arrange
        const string str = "lowercase1";

        // Act
        var outcome = str.ContainsNumber();

        // Assert
        outcome.Should().BeTrue();
    }

    [Fact]
    public void ContainsNumber_WhenStringDoesNotContainsNumber_ShouldReturnFalse()
    {
        // Arrange
        const string str = "no numbers here";

        // Act
        var outcome = str.ContainsNumber();

        // Assert
        outcome.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("l", "L")]
    [InlineData("aNdReI", "Andrei")]
    [InlineData("mIchAel JaMES", "Michael James")]
    [InlineData("CHRISTIAN-MICHAEL", "Christian-Michael")]
    public void Capitalize_ShouldReturnCapitalizedWords(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcomes = input.Capitalize();

        // Assert
        outcomes.Should().Be(expected);
    }
}