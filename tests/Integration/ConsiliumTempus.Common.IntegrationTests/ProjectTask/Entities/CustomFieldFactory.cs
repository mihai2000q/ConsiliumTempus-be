using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.ProjectTask.Entities;

public static class CustomFieldFactory
{
    public static NumberCustomField CreateNumber(
        NumberCustomFieldSetupAggregate customFieldSetup,
        decimal? number = null)
    {
        return EntityBuilder<NumberCustomField>.Empty()
            .WithProperty(nameof(NumberCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(NumberCustomField.Number), number.IfNotNull(DecimalNumber.Create))
            .WithProperty(nameof(NumberCustomField.Setup), customFieldSetup)
            .Build();
    }

    public static SingleSelectCustomField CreateSingleSelect(
        SingleSelectCustomFieldSetupAggregate customFieldSetup,
        SingleSelectOption? option = null)
    {
        return EntityBuilder<SingleSelectCustomField>.Empty()
            .WithProperty(nameof(SingleSelectCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(SingleSelectCustomField.Option), option)
            .WithProperty(nameof(SingleSelectCustomField.Setup), customFieldSetup)
            .Build();
    }

    public static TextCustomField CreateText(
        TextCustomFieldSetupAggregate customFieldSetup,
        string? text = null)
    {
        return EntityBuilder<TextCustomField>.Empty()
            .WithProperty(nameof(TextCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(TextCustomField.Text), text.IfNotNull(Text.Create))
            .WithProperty(nameof(TextCustomField.Setup), customFieldSetup)
            .Build();
    }
}