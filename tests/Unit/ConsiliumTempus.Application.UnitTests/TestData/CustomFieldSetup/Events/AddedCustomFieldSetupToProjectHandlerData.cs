using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Events;

internal static class AddedCustomFieldSetupToProjectHandlerData
{
    internal class GetCustomFieldSetups : TheoryData<CustomFieldSetupAggregate>
    {
        public GetCustomFieldSetups()
        {
            Add(CustomFieldSetupFactory.CreateNumber(project: ProjectFactory.Create()));
            Add(CustomFieldSetupFactory.CreateSingleSelect(project: ProjectFactory.Create()));
            Add(CustomFieldSetupFactory.CreateText(project: ProjectFactory.Create()));
        }
    }
}