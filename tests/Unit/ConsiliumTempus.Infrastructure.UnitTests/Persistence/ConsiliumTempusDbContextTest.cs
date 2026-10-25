using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence;

public class ConsiliumTempusDbContextTest
{
    [Theory]
    [InlineData(typeof(ProjectAggregate))]
    [InlineData(typeof(ProjectSprintAggregate))]
    [InlineData(typeof(ProjectTaskAggregate))]
    [InlineData(typeof(UserAggregate))]
    [InlineData(typeof(WorkspaceAggregate))]
    public void Model_WhenIsBuilt_ShouldApplyTheEntityConfigurations(Type aggregate)
    {
        // Arrange
        // the configurations are internal, this makes sure that they are still discovered
        var options = new DbContextOptionsBuilder<ConsiliumTempusDbContext>()
            .UseSqlServer("Server=.;Database=NotUsed")
            .Options;
        using var dbContext = new ConsiliumTempusDbContext(
            new PublishDomainEventsInterceptor(Substitute.For<IPublisher>()),
            options);

        // Act
        var entityType = dbContext.Model.FindEntityType(aggregate);

        // Assert
        entityType.Should().NotBeNull();
        entityType.GetTableName().Should().Be(aggregate.Name.TruncateAggregate());
    }
}
