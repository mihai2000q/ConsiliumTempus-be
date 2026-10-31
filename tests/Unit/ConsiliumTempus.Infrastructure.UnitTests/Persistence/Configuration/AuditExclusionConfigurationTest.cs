using ConsiliumTempus.Infrastructure.Persistence.Configuration.ValueObjects;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Configuration;

/// <summary>
/// A change that is excluded from the audit does not update it (see <c>StampInterceptor</c>),
/// and it is the only kind of change that <c>ExecuteUpdate</c> is allowed to do on an audited entity.
/// So the exclusions have to be added to the list on purpose, together with their configuration.
/// </summary>
public class AuditExclusionConfigurationTest
{
    private static readonly string[] ExpectedExclusions =
    [
        "ProjectAggregate.LastActivity",
        "ProjectStage.CustomOrderPosition",
        "ProjectTaskAggregate.CustomOrderPosition",
        "WorkspaceAggregate.LastActivity"
    ];

    [Fact]
    public void AuditExclusion_WhenChangesAreExcludedFromTheAudit_ShouldOnlyExcludeTheListedOnes()
    {
        // Arrange
        using var dbContext = Utils.Persistence.CreateDbContext();

        // Act
        var exclusions = dbContext.Model
            .GetEntityTypes()
            .SelectMany(e =>
                e.GetDeclaredProperties().Where(p => p.IsExcludedFromAudit()).Select(p => $"{e.ShortName()}.{p.Name}")
                    .Concat(e.GetDeclaredNavigations().Where(n => n.IsExcludedFromAudit()).Select(n => $"{e.ShortName()}.{n.Name}")));

        // Assert
        exclusions.Should().BeEquivalentTo(ExpectedExclusions);
    }
}