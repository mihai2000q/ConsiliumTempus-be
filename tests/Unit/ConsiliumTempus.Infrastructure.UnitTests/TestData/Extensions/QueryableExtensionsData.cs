using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Extensions;

internal static class QueryableExtensionsData
{
    internal class GetCustomFieldSetups : TheoryData<CustomFieldSetupAggregate, Type>
    {
        public GetCustomFieldSetups()
        {
            Add(CustomFieldSetupFactory.CreateDate(), typeof(DateCustomField));
            Add(CustomFieldSetupFactory.CreateDateTime(), typeof(DateTimeCustomField));
            Add(CustomFieldSetupFactory.CreateDuration(), typeof(DurationCustomField));
            Add(CustomFieldSetupFactory.CreateMultiSelect(), typeof(MultiSelectCustomField));
            Add(CustomFieldSetupFactory.CreateNumber(), typeof(NumberCustomField));
            Add(CustomFieldSetupFactory.CreatePeople(), typeof(PeopleCustomField));
            Add(CustomFieldSetupFactory.CreateSingleSelect(), typeof(SingleSelectCustomField));
            Add(CustomFieldSetupFactory.CreateText(), typeof(TextCustomField));
            Add(CustomFieldSetupFactory.CreateTime(), typeof(TimeCustomField));
        }
    }
}
