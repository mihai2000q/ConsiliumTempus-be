using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTaskAggregate>
{
    public void Configure(EntityTypeBuilder<ProjectTaskAggregate> builder)
    {
        builder.ToTable(nameof(ProjectTaskAggregate).TruncateAggregate());
        
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
            .WithMany();
        
        builder.HasOne(t => t.Asignee)
            .WithMany();
        
        builder.HasOne(t => t.Reviewer)
            .WithMany();

        builder.HasOne(t => t.Section)
            .WithMany(s => s.Tasks);

        builder.OwnsMany(t => t.Comments, ConfigureComments);
    }
    
    private static void ConfigureComments(OwnedNavigationBuilder<ProjectTaskAggregate, ProjectTaskComment> builder)
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

        builder.HasOne(c => c.TaskAggregate)
            .WithMany(t => t.Comments);
    }
}