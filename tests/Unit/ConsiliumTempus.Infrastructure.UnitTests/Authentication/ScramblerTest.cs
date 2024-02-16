using ConsiliumTempus.Infrastructure.Authentication;

namespace ConsiliumTempus.Infrastructure.UnitTests.Authentication;

public class ScramblerTest
{
    #region Setup

    private readonly Scrambler _uut = new();

    #endregion

    [Fact]
    public void WhenHashingPasswordAndVerifyIt_ShouldReturnTrue()
    {
        // Arrange
        const string password = "Password123";

        // Act
        var hashedPassword = _uut.HashPassword(password);
        var outcome = _uut.VerifyPassword(password, hashedPassword);

        // Assert
        outcome.Should().BeTrue();
    }

    [Fact]
    public void WhenHashing2TimesPassword_ShouldNotBeEqual()
    {
        // Arrange
        const string password = "Password123";

        // Act
        var hashedPassword = _uut.HashPassword(password);
        var hashedPassword2 = _uut.HashPassword(password);

        var outcome1 = _uut.VerifyPassword(password, hashedPassword);
        var outcome2 = _uut.VerifyPassword(password, hashedPassword2);

        // Assert - even though the hashes are different, they should both verify the same password
        hashedPassword.Should().NotBe(hashedPassword2);
        outcome1.Should().BeTrue();
        outcome2.Should().BeTrue();
    }
}