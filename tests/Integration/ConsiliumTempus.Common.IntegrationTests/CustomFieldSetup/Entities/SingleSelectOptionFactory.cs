using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;

namespace ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup.Entities;

public static class SingleSelectOptionFactory
{
    public static SingleSelectOption Create(
        string color = Constants.SingleSelectOption.Color,
        string value = Constants.SingleSelectOption.Value1,
        int customOrderPosition = 0)
    {
        return EntityBuilder<SingleSelectOption>.Empty()
            .WithProperty(nameof(SingleSelectOption.Id), Guid.NewGuid())
            .WithProperty(nameof(SingleSelectOption.Color), color)
            .WithProperty(nameof(SingleSelectOption.Value), value)
            .WithProperty(nameof(SingleSelectOption.CustomOrderPosition), customOrderPosition)
            .Build();
    }
}