using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Relations;

internal sealed class ProjectHasCustomFieldSetupConfiguration : IEntityTypeConfiguration<ProjectHasCustomFieldSetup>
{
    public void Configure(EntityTypeBuilder<ProjectHasCustomFieldSetup> builder)
    {
        builder.ToTable(nameof(ProjectHasCustomFieldSetup));

        builder.HasKey(p => new { ProjectId = p.ProjectsId, CustomFieldSetupId = p.CustomFieldSetupsId });
        builder.HasOne<ProjectAggregate>()
            .WithMany()
            .HasForeignKey(p => p.ProjectsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<CustomFieldSetupAggregate>()
            .WithMany()
            .HasForeignKey(p => p.CustomFieldSetupsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.ProjectsId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value))
            .HasColumnName(nameof(ProjectId));

        builder.Property(p => p.CustomFieldSetupsId)
            .HasConversion(
                id => id.Value,
                value => CustomFieldSetupId.Create(value))
            .HasColumnName(nameof(CustomFieldSetupId));
    }
}