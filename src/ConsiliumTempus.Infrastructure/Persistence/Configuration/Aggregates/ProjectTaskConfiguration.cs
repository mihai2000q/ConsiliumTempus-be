using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Aggregates;

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

public sealed class DateCustomFieldConfiguration : IEntityTypeConfiguration<DateCustomField>
{
    public void Configure(EntityTypeBuilder<DateCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dash(CustomFieldType.Date.ToString()));

        builder.Property(d => d.Date);

        builder.HasOne(d => d.Setup)
            .WithMany();
        builder.Navigation(d => d.Setup).AutoInclude();
    }
}

public sealed class DateTimeCustomFieldConfiguration : IEntityTypeConfiguration<DateTimeCustomField>
{
    public void Configure(EntityTypeBuilder<DateTimeCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dash(CustomFieldType.DateTime.ToString()));

        builder.Property(d => d.DateTime);

        builder.HasOne(d => d.Setup)
            .WithMany();
        builder.Navigation(d => d.Setup).AutoInclude();
    }
}

public sealed class DurationCustomFieldConfiguration : IEntityTypeConfiguration<DurationCustomField>
{
    public void Configure(EntityTypeBuilder<DurationCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dash(CustomFieldType.Duration.ToString()));

        builder.Property(d => d.Duration)
            .HasConversion<long>();

        builder.HasOne(d => d.Setup)
            .WithMany();
        builder.Navigation(d => d.Setup).AutoInclude();
    }
}

public sealed class MultiSelectCustomFieldConfiguration : IEntityTypeConfiguration<MultiSelectCustomField>
{
    public void Configure(EntityTypeBuilder<MultiSelectCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dash(CustomFieldType.MultiSelect.ToString()));

        builder.HasMany(m => m.Options)
            .WithMany()
            .UsingEntity<CustomFieldMultiSelectHasOption>();
        builder.Navigation(m => m.Options).AutoInclude();

        builder.HasOne(m => m.Setup)
            .WithMany();
        builder.Navigation(m => m.Setup).AutoInclude();
    }
}

public sealed class NumberCustomFieldConfiguration : IEntityTypeConfiguration<NumberCustomField>
{
    public void Configure(EntityTypeBuilder<NumberCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dash(CustomFieldType.Number.ToString()));

        builder.OwnsOne(n => n.Number)
            .Property(n => n.Value)
            .HasColumnName(nameof(NumberCustomField.Number))
            .HasPrecision(38, PropertiesValidation.CustomFieldSetup.Number.DecimalsMaximum);

        builder.HasOne(n => n.Setup)
            .WithMany();
        builder.Navigation(n => n.Setup).AutoInclude();
    }
}

public sealed class PeopleCustomFieldConfiguration : IEntityTypeConfiguration<PeopleCustomField>
{
    public void Configure(EntityTypeBuilder<PeopleCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dash(CustomFieldType.People.ToString()));

        builder.HasOne(p => p.Person)
            .WithMany();
        builder.Navigation(p => p.Person).AutoInclude();

        builder.HasOne(p => p.Setup)
            .WithMany();
        builder.Navigation(p => p.Setup).AutoInclude();
    }
}

public sealed class SingleSelectCustomFieldConfiguration : IEntityTypeConfiguration<SingleSelectCustomField>
{
    public void Configure(EntityTypeBuilder<SingleSelectCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dash(CustomFieldType.SingleSelect.ToString()));

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
            .Dash(CustomFieldType.Text.ToString()));

        builder.OwnsOne(t => t.Text)
            .Property(n => n.Value)
            .HasColumnName(nameof(TextCustomField.Text));

        builder.HasOne(t => t.Setup)
            .WithMany();
        builder.Navigation(t => t.Setup).AutoInclude();
    }
}

public sealed class TimeCustomFieldConfiguration : IEntityTypeConfiguration<TimeCustomField>
{
    public void Configure(EntityTypeBuilder<TimeCustomField> builder)
    {
        builder.ToTable(nameof(CustomField)
            .Dash(CustomFieldType.Time.ToString()));

        builder.Property(t => t.Time);

        builder.HasOne(t => t.Setup)
            .WithMany();
        builder.Navigation(t => t.Setup).AutoInclude();
    }
}