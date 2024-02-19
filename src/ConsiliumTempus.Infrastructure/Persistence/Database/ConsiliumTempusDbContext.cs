using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConsiliumTempus.Infrastructure.Persistence.Database;

public sealed class ConsiliumTempusDbContext(
    PublishDomainEventInterceptor publishDomainEventInterceptor,
    DbContextOptions<ConsiliumTempusDbContext> options)
    : DbContext(options)
{
    public DbSet<ProjectAggregate> Projects { get; init; } = null!;
    public DbSet<ProjectSprint> ProjectSprints { get; init; } = null!;
    public DbSet<UserAggregate> Users { get; init; } = null!;
    public DbSet<WorkspaceAggregate> Workspaces { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>() // Don't save the list of domain events
            .ApplyConfigurationsFromAssembly(typeof(ConsiliumTempusDbContext).Assembly);

        // Never generate any of the primary keys, let the application generate them
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.IsPrimaryKey())
            .ToList()
            .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(publishDomainEventInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}