using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
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

        builder.OwnsOne(t => t.Name)
            .Property(n => n.Value)
            .HasColumnName(nameof(Name))
            .HasMaxLength(PropertiesValidation.ProjectTask.NameMaximumLength);

        builder.OwnsOne(t => t.Description)
            .Property(d => d.Value)
            .HasColumnName(nameof(Description));

        builder.OwnsOne(t => t.IsCompleted, b =>
        {
            b.Property(c => c.Value)
                .HasColumnName(nameof(IsCompleted));

            b.Property(c => c.CompletedOn)
                .HasColumnName(nameof(IsCompleted.CompletedOn));
        });
            

        builder.OwnsOne(t => t.CustomOrderPosition)
            .Property(o => o.Value)
            .HasColumnName(nameof(CustomOrderPosition));

        builder.HasOne(t => t.CreatedBy)
            .WithMany();

        builder.HasOne(t => t.Assignee)
            .WithMany();
        builder.Navigation(t => t.Assignee).AutoInclude();

        builder.HasOne(t => t.Reviewer)
            .WithMany();

        builder.HasOne(t => t.Stage)
            .WithMany(s => s.Tasks);

        builder.OwnsMany(t => t.Comments, ConfigureComments);
        builder.Navigation(t => t.Comments).AutoInclude(false);
    }

    private static void ConfigureComments(OwnedNavigationBuilder<ProjectTaskAggregate, ProjectTaskComment> builder)
    {
        builder.ToTable(nameof(ProjectTaskComment));

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectTaskCommentId.Create(value));

        builder.OwnsOne(c => c.Message)
            .Property(m => m.Value)
            .HasColumnName(nameof(Message))
            .HasMaxLength(PropertiesValidation.ProjectTaskComment.MessageMaximumLength);

        builder.HasOne(c => c.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Task)
            .WithMany(t => t.Comments);
    }
}