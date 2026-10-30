using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Database;

public class ConsiliumTempusDbContextTest
{
    [Theory]
    [InlineData(typeof(ProjectAggregate))]
    [InlineData(typeof(ProjectSprintAggregate))]
    [InlineData(typeof(ProjectTaskAggregate))]
    [InlineData(typeof(UserAggregate))]
    [InlineData(typeof(WorkspaceAggregate))]
    public void DbContextModel_WhenIsBuilt_ShouldApplyTheEntityConfigurations(Type aggregate)
    {
        // Arrange
        // the configurations are internal, this makes sure that they are still discovered
        using var dbContext = Utils.Persistence.CreateDbContext();

        // Act
        var entityType = dbContext.Model.FindEntityType(aggregate);

        // Assert
        entityType.Should().NotBeNull();
        entityType.GetTableName().Should().Be(aggregate.Name.TruncateAggregate());
    }

    [Fact]
    public void NeverGenerateKeysConvention_WhenTheModelIsBuilt_ShouldBeApplied()
    {
        // Arrange
        // the convention is internal, this makes sure that it is still registered
        using var dbContext = Utils.Persistence.CreateDbContext();

        // Act
        var keys = dbContext.Model
            .GetEntityTypes()
            .SelectMany(e => e.GetDeclaredProperties())
            .Where(p => p.IsPrimaryKey());

        // Assert
        // (the restrictive delete behavior is covered by the test of the configured delete behaviors,
        // since a relationship that would cascade by default is not on its list)
        keys.Should().OnlyContain(p => p.ValueGenerated == ValueGenerated.Never);
    }
}
