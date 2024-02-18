using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class ProjectSprintConfiguration : IEntityTypeConfiguration<ProjectSprint>
{
    public void Configure(EntityTypeBuilder<ProjectSprint> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectSprintId.Create(value));
        
        builder.Property(s => s.Name)
            .HasMaxLength(PropertiesValidation.ProjectSprint.NameMaximumLength);

        builder.HasOne(s => s.Project)
            .WithMany(p => p.Sprints);

        builder.HasMany(s => s.Sections)
            .WithOne(s => s.Sprint);
    }
}