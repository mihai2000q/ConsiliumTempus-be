using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Api.UnitTests.TestData;

internal static class CustomFieldSetupControllerData
{
    internal class GetCustomFieldSetupResults : TheoryData<GetCustomFieldSetupResult>
    {
        public GetCustomFieldSetupResults()
        {
            // Number
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateNumber()));
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateNumber(defaultNumber: 12)));
            
            // Single Select
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateSingleSelect()));

            var options = new List<SingleSelectOption>
            {
                SingleSelectOptionFactory.Create(),
                SingleSelectOptionFactory.Create(),
            };
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateSingleSelect(
                options: options,
                defaultOptionId: options[1].Id)));
            
            // Text
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateText()));
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateText(defaultText: "Something default")));
        }
    }
}