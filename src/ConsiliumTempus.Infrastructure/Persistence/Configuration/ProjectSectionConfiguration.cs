using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class ProjectSectionConfiguration : IEntityTypeConfiguration<ProjectSection>
{
    public void Configure(EntityTypeBuilder<ProjectSection> builder)
    {
        builder.ToTable(nameof(ProjectSection));
        
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectSectionId.Create(value));
        
        builder.Property(s => s.Name)
            .HasMaxLength(PropertiesValidation.ProjectSection.NameMaximumLength);
        
        builder.HasOne(s => s.Sprint)
            .WithMany(sp => sp.Sections);
        
        builder.HasMany(s => s.Tasks)
            .WithOne(s => s.Section);
    }
}