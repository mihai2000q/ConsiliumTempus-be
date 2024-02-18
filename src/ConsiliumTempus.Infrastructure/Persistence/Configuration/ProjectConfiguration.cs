using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<ProjectAggregate>
{
    public void Configure(EntityTypeBuilder<ProjectAggregate> builder)
    {
        builder.ToTable(nameof(ProjectAggregate).TruncateAggregate());

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));
        
        builder.Property(p => p.Name)
            .HasMaxLength(PropertiesValidation.Project.NameMaximumLength);
        
        builder.Property(p => p.Description)
            .HasMaxLength(PropertiesValidation.Project.DescriptionMaximumLength);

        builder.HasOne(p => p.Workspace)
            .WithMany(w => w.Projects);
        
        builder.HasMany(p => p.Sprints)
            .WithOne(s => s.Project);
    }
}