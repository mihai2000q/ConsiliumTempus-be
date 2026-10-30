using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Configuration;

/// <summary>
/// The authorship is stamped by the <c>StampInterceptor</c> on every entity that has one,
/// so every one of them has to map it (see <c>OwnsAuthorship</c>).
/// </summary>
public class AuthorshipConfigurationTest
{
    [Fact]
    public void Authorship_WhenAnEntityIsAuthored_ShouldMapItsAuthorship()
    {
        // Arrange
        using var dbContext = Utils.Persistence.CreateDbContext();
        var authoredTypes = typeof(IAuthored).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IAuthored).IsAssignableFrom(t))
            .ToList();
        authoredTypes.Should().NotBeEmpty();

        // Act
        var notMapped = authoredTypes
            // ReSharper disable once AccessToDisposedClosure
            .Where(t => dbContext.Model.FindEntityType(t)?.FindNavigation(nameof(IAuthored.Authorship)) is not
                { ForeignKey.IsOwnership: true })
            .Select(t => t.Name);

        // Assert
        notMapped.Should().BeEmpty();
    }

    [Fact]
    public void Authorship_WhenAnEntityIsAuthored_ShouldKeepItsAuthorshipInTheTableOfTheEntityWithANullableAuthor()
    {
        // Arrange
        using var dbContext = Utils.Persistence.CreateDbContext();
        var authorships = dbContext.Model
            .GetEntityTypes()
            .Where(e => e.ClrType == typeof(Authorship))
            .ToList();
        authorships.Should().NotBeEmpty();

        // Act
        var invalid = authorships
            .Where(authorship =>
            {
                var owner = authorship.FindOwnership()!.PrincipalEntityType;
                var author = authorship.FindProperty(nameof(Authorship.Author).ToId());

                return authorship.GetTableName() != owner.GetTableName() ||
                       author is not { IsNullable: true } ||
                       author.GetColumnName() != nameof(Authorship.Author).ToId();
            })
            .Select(authorship => authorship.ShortName());

        // Assert
        invalid.Should().BeEmpty();
    }
}
