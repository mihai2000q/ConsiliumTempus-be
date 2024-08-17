using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
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

        builder.HasMany(t => t.CustomFields)
            .WithOne(cf => cf.ProjectTask)
            .OnDelete(DeleteBehavior.NoAction);
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

public sealed class CustomFieldConfiguration : IEntityTypeConfiguration<CustomField>
{
    public void Configure(EntityTypeBuilder<CustomField> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.HasKey(cf => cf.Id);
        builder.Property(cf => cf.Id)
            .HasConversion(
                id => id.Value,
                value => CustomFieldId.Create(value));

        builder.HasOne(cf => cf.ProjectTask)
            .WithMany(t => t.CustomFields);
    }
}

public sealed class NumberCustomFieldConfiguration : IEntityTypeConfiguration<NumberCustomField>
{
    public void Configure(EntityTypeBuilder<NumberCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dot(nameof(NumberCustomField).Replace(nameof(CustomField), "")));

        builder.OwnsOne(ncf => ncf.Number)
            .Property(n => n.Value)
            .HasColumnName(nameof(NumberCustomField.Number))
            .HasPrecision(38, PropertiesValidation.CustomFieldSetup.Number.DecimalsMaximum);

        builder.HasOne(ncf => ncf.Setup)
            .WithMany();
        builder.Navigation(ncf => ncf.Setup).AutoInclude();
    }
}

public sealed class SingleSelectCustomFieldConfiguration : IEntityTypeConfiguration<SingleSelectCustomField>
{
    public void Configure(EntityTypeBuilder<SingleSelectCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dot(nameof(SingleSelectCustomField).Replace(nameof(CustomField), "")));

        builder.HasOne(s => s.Option)
            .WithMany();
        builder.Navigation(s => s.Option).AutoInclude();

        builder.HasOne(s => s.Setup)
            .WithMany();
        builder.Navigation(s => s.Setup).AutoInclude();
    }
}

public sealed class TextCustomFieldConfiguration : IEntityTypeConfiguration<TextCustomField>
{
    public void Configure(EntityTypeBuilder<TextCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dot(nameof(TextCustomField).Replace(nameof(CustomField), "")));

        builder.OwnsOne(tcf => tcf.Text)
            .Property(n => n.Value)
            .HasColumnName(nameof(TextCustomField.Text));

        builder.HasOne(t => t.Setup)
            .WithMany();
        builder.Navigation(tcf => tcf.Setup).AutoInclude();
    }
}