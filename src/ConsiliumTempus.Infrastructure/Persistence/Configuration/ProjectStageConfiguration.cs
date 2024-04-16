using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class ProjectStageConfiguration : IEntityTypeConfiguration<ProjectStage>
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

        builder.OwnsOne(s => s.Order)
            .Property(o => o.Value)
            .HasColumnName(nameof(Order));

        builder.HasOne(s => s.Sprint)
            .WithMany(sp => sp.Stages);

        builder.HasMany(s => s.Tasks)
            .WithOne(s => s.Stage);
    }
}