using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Common.UnitTests.Common.Entities;

public static class SingleSelectOptionFactory
{
    public static SingleSelectOption Create(
        string value = Constants.SingleSelectOption.Value1,
        string color = Constants.SingleSelectOption.Color,
        int orderPosition = 0)
    {
        return SingleSelectOption.Create(
            value,
            color,
            orderPosition);
    }
}