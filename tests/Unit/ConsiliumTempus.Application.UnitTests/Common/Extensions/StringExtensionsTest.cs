using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.UnitTests.TestData.Common.Extensions;

namespace ConsiliumTempus.Application.UnitTests.Common.Extensions;

public class StringExtensionsTest
{
    [Theory]
    [ClassData(typeof(StringExtensionsData.GetValidToId))]
    public void ToBackingField_WhenValid_ShouldReturnStringWithIdNotation(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome1 = input.ToId();

        // Assert
        outcome1.Should().Be(expected);
    }

    [Theory]
    [ClassData(typeof(StringExtensionsData.GetValidToBackingField))]
    public void ToBackingField_WhenValid_ShouldReturnStringWithBackingFieldNotation(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome1 = input.ToBackingField();

        // Assert
        outcome1.Should().Be(expected);
    }

    [Theory]
    [ClassData(typeof(StringExtensionsData.GetValidToIdBackingField))]
    public void ToIdBackingField_WhenValid_ShouldReturnStringWithIdBackingFieldNotation(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome1 = input.ToIdBackingField();

        // Assert
        outcome1.Should().Be(expected);
    }

    [Theory]
    [ClassData(typeof(StringExtensionsData.GetValidTruncateAggregate))]
    public void TruncateAggregate_WhenValid_ShouldReturnStringWithoutAggregate(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcome1 = input.TruncateAggregate();

        // Assert
        outcome1.Should().Be(expected);
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

    [Fact]
    public void Capitalize_WhenLengthIs0_ShouldReturnEmpty()
    {
        // Arrange
        const string input = "";

        // Act
        var outcome = input.Capitalize();

        // Assert
        outcome.Should().BeEmpty();
    }

    [Fact]
    public void Capitalize_WhenLengthIs1_ShouldReturnUpper()
    {
        // Arrange
        const string input = "l";

        // Act
        var outcome = input.Capitalize();

        // Assert
        outcome.Should().Be("L");
    }

    [Theory]
    [ClassData(typeof(StringExtensionsData.GetCapitalizeStrings))]
    public void Capitalize_WhenLengthIsOver1_ShouldReturnCapitalizedWords(string input, string expected)
    {
        // Arrange - parameterized

        // Act
        var outcomes = input.Capitalize();

        // Assert
        outcomes.Should().Be(expected);
    }
}