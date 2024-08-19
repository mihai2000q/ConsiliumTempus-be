using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask.Entities;

public static class CustomFieldFactory
{
    public static NumberCustomField CreateNumber(
        decimal? number = null,
        NumberCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return NumberCustomField.Create(
            number.IfNotNull(DecimalNumber.Create),
            setup ?? CustomFieldSetupFactory.CreateNumber(),
            task ?? ProjectTaskFactory.Create());
    }

    public static SingleSelectCustomField CreateSingleSelect(
        Guid? optionId = null,
        SingleSelectCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        setup ??= CustomFieldSetupFactory.CreateSingleSelect();
        var option = setup.Options.SingleOrDefault(o => o.Id == optionId);

        return SingleSelectCustomField.Create(
            option,
            setup,
            task ?? ProjectTaskFactory.Create());
    }
    
    public static TextCustomField CreateText(
        string? text = null,
        TextCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return TextCustomField.Create(
            text.IfNotNull(Text.Create),
            setup ?? CustomFieldSetupFactory.CreateText(),
            task ?? ProjectTaskFactory.Create());
    }
}