using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Common.UnitTests.Common.Entities;

public static class MultiSelectOptionFactory
{
    public static MultiSelectOption Create(
        string value = Constants.MultiSelectOption.Value1,
        string color = Constants.MultiSelectOption.Color,
        int customOrderPosition = 0)
    {
        return MultiSelectOption.Create(
            value,
            color,
            CustomOrderPosition.Create(customOrderPosition));
    }
}