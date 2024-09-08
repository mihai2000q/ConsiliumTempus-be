using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Common.UnitTests.Common.Entities;

public static class SingleSelectOptionFactory
{
    public static SingleSelectOption Create(
        string value = Constants.SingleSelectOption.Value1,
        string color = Constants.SingleSelectOption.Color,
        int customOrderPosition = 0)
    {
        return SingleSelectOption.Create(
            value,
            color,
            CustomOrderPosition.Create(customOrderPosition));
    }
}