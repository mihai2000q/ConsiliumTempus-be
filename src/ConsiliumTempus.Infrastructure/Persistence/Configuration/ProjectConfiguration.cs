using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
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
        
        builder.OwnsMany(p => p.Sprints, ConfigureSprints);
    }

    private static void ConfigureSprints(OwnedNavigationBuilder<ProjectAggregate, ProjectSprint> builder)
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

        builder.OwnsMany(s => s.Sections, ConfigureSections);
    }
    
    private static void ConfigureSections(OwnedNavigationBuilder<ProjectSprint, ProjectSection> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectSectionId.Create(value));
        
        builder.Property(s => s.Name)
            .HasMaxLength(PropertiesValidation.ProjectSection.NameMaximumLength);
        
        builder.HasOne(s => s.Sprint)
            .WithMany(sp => sp.Sections);

        builder.OwnsMany(s => s.Tasks, ConfigureTasks);
    }
    
    private static void ConfigureTasks(OwnedNavigationBuilder<ProjectSection, ProjectTask> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectTaskId.Create(value));
        
        builder.Property(t => t.Name)
            .HasMaxLength(PropertiesValidation.ProjectTask.NameMaximumLength);
        
        builder.Property(t => t.Description)
            .HasMaxLength(PropertiesValidation.ProjectTask.DescriptionMaximumLength);

        builder.HasOne(t => t.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(t => t.Asignee)
            .WithMany();
        
        builder.HasOne(t => t.Reviewer)
            .WithMany();

        builder.HasOne(t => t.Section)
            .WithMany(s => s.Tasks);

        builder.OwnsMany(t => t.Comments, ConfigureComments);
    }

    private static void ConfigureComments(OwnedNavigationBuilder<ProjectTask, ProjectTaskComment> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectTaskCommentId.Create(value));
        
        builder.Property(c => c.Message)
            .HasMaxLength(PropertiesValidation.ProjectTaskComment.MessageMaximumLength);

        builder.HasOne(c => c.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Task)
            .WithMany(t => t.Comments);
    }
}