using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class CustomFieldSetupConfiguration : IEntityTypeConfiguration<CustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<CustomFieldSetupAggregate> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.HasKey(cfs => cfs.Id);
        builder.Property(cfs => cfs.Id)
            .HasConversion(
                id => id.Value,
                value => CustomFieldSetupId.Create(value));

        builder.Property(cfs => cfs.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value))
            .HasColumnName(nameof(Name))
            .HasMaxLength(PropertiesValidation.CustomFieldSetup.NameMaximumLength);

        builder.Property(cfs => cfs.Description)
            .HasConversion(
                description => description.Value,
                value => Description.Create(value))
            .HasColumnName(nameof(Description));

        builder.HasOne(cfs => cfs.Workspace)
            .WithMany(w => w.CustomFieldSetups)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(cfs => cfs.Projects)
            .WithMany(p => p.CustomFieldSetups)
            .UsingEntity("ProjectHasCustomFieldSetup");

        builder.HasOne(s => s.Audit)
            .WithOne()
            .HasForeignKey<CustomFieldSetupAggregate>()
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Navigation(cfs => cfs.Audit).AutoInclude();
    }
}

public sealed class DateCustomFieldSetupConfiguration : IEntityTypeConfiguration<DateCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<DateCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.Date.ToString()));

        builder.Property(d => d.DefaultDate);
    }
}

public sealed class DateTimeCustomFieldSetupConfiguration : IEntityTypeConfiguration<DateTimeCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<DateTimeCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.DateTime.ToString()));

        builder.Property(d => d.DefaultDateTime);
    }
}

public sealed class DurationCustomFieldSetupConfiguration : IEntityTypeConfiguration<DurationCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<DurationCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.Duration.ToString()));

        builder.Property(d => d.DefaultDuration);
    }
}

public sealed class
    MultiSelectCustomFieldSetupConfiguration : IEntityTypeConfiguration<MultiSelectCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<MultiSelectCustomFieldSetupAggregate> builder)
    {
        var tableName = nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.MultiSelect.ToString());
        builder.ToTable(tableName);

        builder.HasMany(m => m.Options)
            .WithOne()
            .HasForeignKey("SetupId")
            .IsRequired();
        builder.Navigation(m => m.Options).AutoInclude();
    }
}

public sealed class NumberCustomFieldSetupConfiguration : IEntityTypeConfiguration<NumberCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<NumberCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.Number.ToString()));

        builder.OwnsOne(n => n.Settings, nb =>
        {
            nb.Property(n => n.CurrencyCode)
                .HasColumnName(nameof(NumberCustomFieldSettings.CurrencyCode));

            nb.Property(n => n.Decimals)
                .HasColumnName(nameof(NumberCustomFieldSettings.Decimals));

            nb.Property(n => n.Rounding)
                .HasColumnName(nameof(NumberCustomFieldSettings.Rounding));
        });

        builder.OwnsOne(n => n.DefaultNumber)
            .Property(dn => dn.Value)
            .HasPrecision(38, PropertiesValidation.CustomFieldSetup.Number.DecimalsMaximum)
            .HasColumnName(nameof(NumberCustomFieldSetupAggregate.DefaultNumber));
    }
}

public sealed class PeopleCustomFieldSetupConfiguration : IEntityTypeConfiguration<PeopleCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<PeopleCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.People.ToString()));
    }
}

public sealed class
    SingleSelectCustomFieldSetupConfiguration : IEntityTypeConfiguration<SingleSelectCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<SingleSelectCustomFieldSetupAggregate> builder)
    {
        var tableName = nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.SingleSelect.ToString());
        builder.ToTable(tableName);

        builder.HasMany(s => s.Options)
            .WithOne()
            .HasForeignKey("SetupId")
            .IsRequired();
        builder.Navigation(s => s.Options).AutoInclude();

        // Circular Dependency if foreign key
        builder.Property(s => s.DefaultOptionId)
            .IsRequired(false);
    }
}

public sealed class TextCustomFieldSetupConfiguration : IEntityTypeConfiguration<TextCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<TextCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.Text.ToString()));

        builder.OwnsOne(n => n.DefaultText)
            .Property(dt => dt.Value)
            .HasColumnName(nameof(TextCustomFieldSetupAggregate.DefaultText));
    }
}

public sealed class TimeCustomFieldSetupConfiguration : IEntityTypeConfiguration<TimeCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<TimeCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dash(CustomFieldType.Time.ToString()));

        builder.Property(t => t.DefaultTime);
    }
}