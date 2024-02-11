using ConsiliumTempus.Application.Common.Extensions;

namespace ConsiliumTempus.Application.UnitTests.Common.Extensions;

public class StringExtensionsTest
{
    [Fact]
    public void TruncateAggregate_WhenValid_ShouldReturnTheStringWithoutAggregate()
    {
        // Arrange
        const string input1 = "WorkspaceAggregate";
        const string input2 = "Workspace";

        // Act
        var outcome1 = input1.TruncateAggregate();
        var outcome2 = input2.TruncateAggregate();
        var outcome3 = "".TruncateAggregate();

        // Assert
        outcome1.Should().Be("Workspace");
        outcome2.Should().Be("Workspace");
        outcome3.Should().BeEmpty();
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
    public void IsValidEmail_WhenStringIsValidEmail_ShouldReturnTrue()
    {
        // Arrange
        const string str = "Some@Example.com";

        // Act
        var outcome = str.IsValidEmail();

        // Assert
        outcome.Should().BeTrue();
    }

    [Fact]
    public void IsValidEmail_WhenStringIsNotValidEmail_ShouldReturnFalse()
    {
        // Arrange
        var input = new[] { "SomeExample", "Some@Example", "SomeExample.com" };

        // Act
        var outcome = input.Select(s => s.IsValidEmail());

        // Assert
        outcome.Should().AllBeEquivalentTo(false);
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

    [Fact]
    public void Capitalize_WhenLengthIsOver1_ShouldReturnCapitalizedWords()
    {
        // Arrange
        var inputs = new[] { "aNdReI", "mIchAel JaMES", "CHRISTIAN-MICHAEL" };
        var expectedOutcomes = new[] { "Andrei", "Michael James", "Christian-Michael" };

        // Act
        var outcomes = inputs.Select(s => s.Capitalize());

        // Assert
        outcomes.Should().BeEquivalentTo(expectedOutcomes);
    }
}