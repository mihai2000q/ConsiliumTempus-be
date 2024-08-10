using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;
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
            .WithMany(w => w.CustomFieldSetups);

        builder.HasOne(cfs => cfs.Project)
            .WithMany(p => p.CustomFieldSetups);

        builder.HasOne(cfs => cfs.Audit)
            .WithMany();
        builder.Navigation(cfs => cfs.Audit).AutoInclude();
    }
}

public sealed class NumberCustomFieldSetupConfiguration : IEntityTypeConfiguration<NumberCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<NumberCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dot(CustomFieldType.Number.ToString()));

        builder.OwnsOne(n => n.Settings, nb =>
        {
            nb.Property(n => n.CurrencyCode)
                .HasColumnName(nameof(NumberCustomFieldSettings.CurrencyCode));

            nb.Property(n => n.Decimals)
                .HasColumnName(nameof(NumberCustomFieldSettings.Decimals));

            nb.Property(n => n.Rounding)
                .HasColumnName(nameof(NumberCustomFieldSettings.Rounding));
        });
    }
}

public sealed class SingleSelectCustomFieldSetupConfiguration : IEntityTypeConfiguration<SingleSelectCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<SingleSelectCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dot(CustomFieldType.SingleSelect.ToString()));

        builder.OwnsMany(s => s.Options, ConfigureOptions);
    }

    private static void ConfigureOptions(OwnedNavigationBuilder<SingleSelectCustomFieldSetupAggregate, SingleSelectOption> builder)
    {
        builder.ToTable(nameof(SingleSelectOption));

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Value)
            .HasMaxLength(PropertiesValidation.SingleSelectOption.ValueMaximumLength);

        builder.Property(o => o.Color);
        builder.Property(o => o.CustomOrderPosition);
    }
}

public sealed class TextCustomFieldSetupConfiguration : IEntityTypeConfiguration<TextCustomFieldSetupAggregate>
{
    public void Configure(EntityTypeBuilder<TextCustomFieldSetupAggregate> builder)
    {
        builder.ToTable(nameof(CustomFieldSetupAggregate)
            .TruncateAggregate()
            .Dot(CustomFieldType.Text.ToString()));
    }
}