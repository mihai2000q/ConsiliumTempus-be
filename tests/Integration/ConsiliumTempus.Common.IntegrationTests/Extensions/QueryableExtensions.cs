using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Common.IntegrationTests.Extensions;

// Copy of the (internal) extension from the Infrastructure, so the tests do not need access to its internals
public static class QueryableExtensions
{
    public static IQueryable<CustomField> OfCustomFieldType(
        this IQueryable<CustomField> queryable,
        CustomFieldSetupAggregate customFieldSetup)
    {
        return customFieldSetup switch
        {
            DateCustomFieldSetupAggregate => queryable.OfType<DateCustomField>(),
            DateTimeCustomFieldSetupAggregate => queryable.OfType<DateTimeCustomField>(),
            DurationCustomFieldSetupAggregate => queryable.OfType<DurationCustomField>(),
            MultiSelectCustomFieldSetupAggregate => queryable.OfType<MultiSelectCustomField>(),
            NumberCustomFieldSetupAggregate => queryable.OfType<NumberCustomField>(),
            PeopleCustomFieldSetupAggregate => queryable.OfType<PeopleCustomField>(),
            SingleSelectCustomFieldSetupAggregate => queryable.OfType<SingleSelectCustomField>(),
            TextCustomFieldSetupAggregate => queryable.OfType<TextCustomField>(),
            TimeCustomFieldSetupAggregate => queryable.OfType<TimeCustomField>(),
            _ => throw new ArgumentOutOfRangeException(nameof(customFieldSetup), customFieldSetup, "Type Not Supported")
        };
    }
}
