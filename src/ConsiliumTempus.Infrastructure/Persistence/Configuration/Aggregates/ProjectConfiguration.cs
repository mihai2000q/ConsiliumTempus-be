using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Configuration.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Aggregates;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<ProjectAggregate>
{
    public void Configure(EntityTypeBuilder<ProjectAggregate> builder)
    {
        builder.ToTable(nameof(ProjectAggregate).TruncateAggregate());

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));

        builder.OwnsOne(p => p.Name)
            .Property(n => n.Value)
            .HasColumnName(nameof(Name))
            .HasMaxLength(PropertiesValidation.Project.NameMaximumLength);

        builder.OwnsOne(p => p.Description)
            .Property(d => d.Value)
            .HasColumnName(nameof(Description));

        builder.OwnsOne(p => p.IsPrivate)
            .Property(p => p.Value)
            .HasColumnName(nameof(IsPrivate));

        builder.HasOne(p => p.Owner)
            .WithMany();
        builder.Navigation(p => p.Owner).AutoInclude();

        builder.HasOne(p => p.Workspace)
            .WithMany(w => w.Projects)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(p => p.Workspace).AutoInclude();

        builder.OwnsAudit(p => p.Audit);

        builder.Property(p => p.LastActivity)
            .ExcludeFromAudit();

        builder.HasMany(p => p.Statuses)
            .WithOne(ps => ps.Project);

        builder.HasMany(p => p.Favorites)
            .WithMany()
            .UsingEntity<UserHasFavoriteProject>();

        builder.HasMany(p => p.AllowedMembers)
            .WithMany()
            .UsingEntity<ProjectHasAllowedMember>();
    }
}

internal sealed class ProjectStatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
{
    public void Configure(EntityTypeBuilder<ProjectStatus> builder)
    {
        builder.ToTable(nameof(ProjectStatus));

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectStatusId.Create(value));

        builder.OwnsOne(s => s.Title)
            .Property(t => t.Value)
            .HasColumnName(nameof(Title))
            .HasMaxLength(PropertiesValidation.ProjectStatus.TitleMaximumLength);

        builder.OwnsOne(s => s.Description)
            .Property(d => d.Value)
            .HasColumnName(nameof(Description));

        builder.HasOne(s => s.Project)
            .WithMany(t => t.Statuses)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsAudit(s => s.Audit);
    }
}