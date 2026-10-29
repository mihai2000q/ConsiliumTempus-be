using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Configuration;

/// <summary>
/// The audit is stamped by the <c>StampAuditInterceptor</c> on every entity that has one,
/// so every one of them has to map it (see <c>OwnsAudit</c>).
/// </summary>
public class AuditConfigurationTest
{
    [Fact]
    public void Audit_WhenAnEntityIsAudited_ShouldMapItsAudit()
    {
        // Arrange
        using var dbContext = Utils.Persistence.CreateDbContext();
        var auditedTypes = typeof(IAudit).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IAudit).IsAssignableFrom(t))
            .ToList();
        auditedTypes.Should().NotBeEmpty();

        // Act
        var notMapped = auditedTypes
            // ReSharper disable once AccessToDisposedClosure
            .Where(t => dbContext.Model.FindEntityType(t)?.FindNavigation(nameof(IAudit.Audit)) is not
                { ForeignKey.IsOwnership: true })
            .Select(t => t.Name);

        // Assert
        notMapped.Should().BeEmpty();
    }

    [Fact]
    public void Audit_WhenAnEntityIsAudited_ShouldKeepItsAuditInTheTableOfTheEntityWithNullableUsers()
    {
        // Arrange
        using var dbContext = Utils.Persistence.CreateDbContext();
        var audits = dbContext.Model
            .GetEntityTypes()
            .Where(e => e.ClrType == typeof(Audit))
            .ToList();
        audits.Should().NotBeEmpty();

        // Act
        var invalid = audits
            .Where(audit =>
            {
                var owner = audit.FindOwnership()!.PrincipalEntityType;
                var createdBy = audit.FindProperty(nameof(Audit.CreatedBy).ToId());
                var updatedBy = audit.FindProperty(nameof(Audit.UpdatedBy).ToId());

                return audit.GetTableName() != owner.GetTableName() ||
                       createdBy is not { IsNullable: true } || updatedBy is not { IsNullable: true } ||
                       createdBy.GetColumnName() != nameof(Audit.CreatedBy).ToId() ||
                       updatedBy.GetColumnName() != nameof(Audit.UpdatedBy).ToId();
            })
            .Select(audit => audit.ShortName());

        // Assert
        invalid.Should().BeEmpty();
    }
}
