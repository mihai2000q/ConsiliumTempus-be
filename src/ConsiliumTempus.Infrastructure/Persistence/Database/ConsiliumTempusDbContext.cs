using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Configuration.Relations;
using ConsiliumTempus.Infrastructure.Persistence.Conventions;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConsiliumTempus.Infrastructure.Persistence.Database;

public sealed class ConsiliumTempusDbContext(
    PublishDomainEventsInterceptor publishDomainEventsInterceptor,
    DbContextOptions<ConsiliumTempusDbContext> options)
    : DbContext(options)
{
    public DbSet<CustomFieldSetupAggregate> CustomFieldSetups { get; init; } = null!;
    public DbSet<ProjectAggregate> Projects { get; init; } = null!;
    public DbSet<ProjectSprintAggregate> ProjectSprints { get; init; } = null!;
    public DbSet<ProjectTaskAggregate> ProjectTasks { get; init; } = null!;
    public DbSet<UserAggregate> Users { get; init; } = null!;
    public DbSet<WorkspaceAggregate> Workspaces { get; init; } = null!;

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new RestrictDeleteBehaviorConvention());
        configurationBuilder.Conventions.Add(_ => new NeverGenerateKeysConvention());

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(ConsiliumTempusDbContext).Assembly;
        var relationsNamespace = typeof(CustomFieldMultiSelectHasOptionConfiguration).Namespace;

        modelBuilder
            .Ignore<List<IDomainEvent>>() // Don't save the list of domain events
            // The joint entities go first, so that the many-to-many relationships find their foreign keys
            .ApplyConfigurationsFromAssembly(assembly, t => t.Namespace == relationsNamespace)
            .ApplyConfigurationsFromAssembly(assembly, t => t.Namespace != relationsNamespace);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}