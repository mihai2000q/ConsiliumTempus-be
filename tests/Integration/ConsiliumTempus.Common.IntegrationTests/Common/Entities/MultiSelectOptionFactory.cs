using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Common.Entities;

public static class MultiSelectOptionFactory
{
    public static MultiSelectOption Create(
        string color = Constants.MultiSelectOption.Color,
        string value = Constants.MultiSelectOption.Value,
        int customOrderPosition = 0)
    {
        return EntityBuilder<MultiSelectOption>.Empty()
            .WithProperty(nameof(MultiSelectOption.Id), Guid.NewGuid())
            .WithProperty(nameof(MultiSelectOption.Color), color)
            .WithProperty(nameof(MultiSelectOption.Value), value)
            .WithProperty(nameof(MultiSelectOption.CustomOrderPosition), CustomOrderPosition.Create(customOrderPosition))
            .Build();
    }
}