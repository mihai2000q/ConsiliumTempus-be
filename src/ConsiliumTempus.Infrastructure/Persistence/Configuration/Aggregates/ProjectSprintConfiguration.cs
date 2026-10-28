using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Configuration.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Aggregates;

internal sealed class ProjectSprintConfiguration : IEntityTypeConfiguration<ProjectSprintAggregate>
{
    public void Configure(EntityTypeBuilder<ProjectSprintAggregate> builder)
    {
        builder.ToTable(nameof(ProjectSprintAggregate).TruncateAggregate());

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectSprintId.Create(value));

        builder.OwnsOne(s => s.Name)
            .Property(n => n.Value)
            .HasColumnName(nameof(Name))
            .HasMaxLength(PropertiesValidation.ProjectSprint.NameMaximumLength);

        builder.HasOne(s => s.Project)
            .WithMany(p => p.Sprints)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Stages)
            .WithOne(s => s.Sprint);

        builder.OwnsAudit(s => s.Audit);
    }
}

internal sealed class ProjectStageConfiguration : IEntityTypeConfiguration<ProjectStage>
{
    public void Configure(EntityTypeBuilder<ProjectStage> builder)
    {
        builder.ToTable(nameof(ProjectStage));

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectStageId.Create(value));

        builder.OwnsOne(s => s.Name)
            .Property(n => n.Value)
            .HasColumnName(nameof(Name))
            .HasMaxLength(PropertiesValidation.ProjectStage.NameMaximumLength);

        builder.OwnsOne(s => s.CustomOrderPosition)
            .Property(o => o.Value)
            .HasColumnName(nameof(CustomOrderPosition));

        builder.HasOne(s => s.Sprint)
            .WithMany(ps => ps.Stages)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Tasks)
            .WithOne(s => s.Stage);

        builder.OwnsAudit(s => s.Audit);
    }
}