using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class ProjectSprintConfiguration : IEntityTypeConfiguration<ProjectSprint>
{
    public void Configure(EntityTypeBuilder<ProjectSprint> builder)
    {
        builder.ToTable(nameof(ProjectSprint));

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
            .WithMany(p => p.Sprints);

        builder.HasMany(s => s.Stages)
            .WithOne(s => s.Sprint);
    }
}