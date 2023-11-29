using ConsiliumTempus.Application.Common.Extensions;
using FluentAssertions;

namespace ConsiliumTempus.Application.UnitTests.Common.Extensions;

public class StringExtensionsTest
{
    [Fact]
    public void WhenStringContainsUppercaseLetter_ShouldReturnTrue()
    {
        // Arrange
        const string str = "Uppercase";
        
        // Act
        var outcome = str.ContainsUppercase();

        // Assert
        outcome.Should().BeTrue();
    }
    
    [Fact]
    public void WhenStringDoesNotContainUppercaseLetter_ShouldReturnFalse()
    {
        // Arrange
        const string str = "uppercase";
        
        // Act
        var outcome = str.ContainsUppercase();

        // Assert
        outcome.Should().BeFalse();
    }
    
    [Fact]
    public void WhenStringContainsLowercaseLetter_ShouldReturnTrue()
    {
        // Arrange
        const string str = "lowercase";
        
        // Act
        var outcome = str.ContainsLowercase();

        // Assert
        outcome.Should().BeTrue();
    }
    
    [Fact]
    public void WhenStringDoesNotContainLowercaseLetter_ShouldReturnFalse()
    {
        // Arrange
        const string str = "LOWERCASE";
        
        // Act
        var outcome = str.ContainsLowercase();

        // Assert
        outcome.Should().BeFalse();
    }
    
    [Fact]
    public void WhenStringContainsNumber_ShouldReturnTrue()
    {
        // Arrange
        const string str = "lowercase1";
        
        // Act
        var outcome = str.ContainsNumber();

        // Assert
        outcome.Should().BeTrue();
    }
    
    [Fact]
    public void WhenStringDoesNotContainsNumber_ShouldReturnFalse()
    {
        // Arrange
        const string str = "no numbers here";
        
        // Act
        var outcome = str.ContainsNumber();

        // Assert
        outcome.Should().BeFalse();
    }
    
    [Fact]
    public void WhenStringIsValidEmail_ShouldReturnTrue()
    {
        // Arrange
        const string str = "Some@Example.com";
        
        // Act
        var outcome = str.IsValidEmail();

        // Assert
        outcome.Should().BeTrue();
    }
    
    [Fact]
    public void WhenStringIsNotValidEmail_ShouldReturnFalse()
    {
        // Arrange
        var input = new[] { "SomeExample", "Some@Example", "SomeExample.com" };
        
        // Act
        var outcome = input.Select(s => s.IsValidEmail());

        // Assert
        outcome.Should().AllBeEquivalentTo(false);
    }
}