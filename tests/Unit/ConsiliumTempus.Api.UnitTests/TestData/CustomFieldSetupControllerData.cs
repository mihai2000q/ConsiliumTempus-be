using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.UnitTests.TestData;

internal static class CustomFieldSetupControllerData
{
    internal class GetCustomFieldSetupResults : TheoryData<GetCustomFieldSetupResult>
    {
        public GetCustomFieldSetupResults()
        {
            // Date
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateDate()));
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateDate(
                defaultDate: new DateOnly(2022, 10, 10))));

            // Date Time
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateDateTime()));
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateDateTime(
                defaultDateTime: new DateTime(2022, 10, 10, 10, 55, 30))));

            // Duration
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateDuration()));
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateDuration(
                defaultDuration: new TimeSpan(10, 15, 30, 45))));

            // Multi Select
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateMultiSelect()));
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateMultiSelect(
                options:
                [
                    MultiSelectOptionFactory.Create(),
                    MultiSelectOptionFactory.Create(customOrderPosition: 1)
                ])));

            // Number
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateNumber()));
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateNumber(defaultNumber: 12)));

            // People
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreatePeople()));

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

            // Time
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateTime()));
            Add(new GetCustomFieldSetupResult(CustomFieldSetupFactory.CreateTime(defaultTime: new TimeOnly(10, 55))));
        }
    }

    internal class GetCreateOnWorkspaceRequests : TheoryData<CreateCustomFieldSetupOnWorkspaceRequest>
    {
        public GetCreateOnWorkspaceRequests()
        {
            // Date
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest(
                    new DateOnly(2022, 10, 10))));

            // Date Time
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest(
                    new DateTime(2022, 10, 10, 10, 55, 30))));

            // Duration
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest(
                    new TimeSpan(10, 15, 30, 45))));

            // Multi Select
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest([])));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                    [
                        new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest(
                            "First", "#FF2233"),
                        new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest(
                            "Second", "#33FF22")
                    ])));

            // Number
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                    new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                        null, 0, false),
                    null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                    new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                        "USD", 2, true),
                    12)));

            // People
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.People));

            // Single Select
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    [], null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    [
                        new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                            "first-id", "First", "#FF2233"),
                        new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                            "second-id", "Second", "#33FF22")
                    ],
                    "second-id")));

            // Text
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Text,
                textCustomFieldSetup: new CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Text,
                textCustomFieldSetup: new CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest("Some text")));

            // Time
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest(
                    new TimeOnly(10, 55))));
        }
    }

    internal class GetCreateOnProjectRequests : TheoryData<CreateCustomFieldSetupOnProjectRequest>
    {
        public GetCreateOnProjectRequests()
        {
            // Date
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest(
                    new DateOnly(2022, 10, 10))));

            // Date Time
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest(
                    new DateTime(2022, 10, 10, 10, 55, 30))));

            // Duration
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest(
                    new TimeSpan(10, 15, 30, 45))));

            // Multi Select
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest([])));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                    [
                        new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest(
                            "First", "#FF2233"),
                        new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest(
                            "Second", "#33FF22")
                    ])));

            // Number
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                    new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                        null, 0, false),
                    null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                    new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                        "USD", 2, true),
                    12)));

            // People
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.People));

            // Single Select
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    [], null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    [
                        new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                            "first-id", "First", "#FF2233"),
                        new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                            "second-id", "Second", "#33FF22")
                    ],
                    "second-id")));

            // Text
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Text,
                textCustomFieldSetup: new CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Text,
                textCustomFieldSetup: new CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest("Some text")));

            // Time
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest(
                    new TimeOnly(10, 55))));
        }
    }

    internal class GetUpdateRequests : TheoryData<UpdateCustomFieldSetupRequest>
    {
        public GetUpdateRequests()
        {
            // Date
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new UpdateCustomFieldSetupRequest.DateCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new UpdateCustomFieldSetupRequest.DateCustomFieldSetupRequest(
                    new DateOnly(2022, 10, 10))));

            // Date Time
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new UpdateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new UpdateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest(
                    new DateTime(2022, 10, 10, 10, 55, 30))));

            // Duration
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new UpdateCustomFieldSetupRequest.DurationCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new UpdateCustomFieldSetupRequest.DurationCustomFieldSetupRequest(
                    new TimeSpan(10, 15, 30, 45))));

            // Multi Select
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                    null, null, null, null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                    UpdateCustomFieldSetupRequest.OptionOperation.Add,
                    new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest(
                        "#FF2233", "High"),
                    null,
                    null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                    UpdateCustomFieldSetupRequest.OptionOperation.Update,
                    new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest(
                        "#FF2233", "High"),
                    Guid.NewGuid(),
                    null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                    UpdateCustomFieldSetupRequest.OptionOperation.Move,
                    null,
                    Guid.NewGuid(),
                    Guid.NewGuid())));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                    UpdateCustomFieldSetupRequest.OptionOperation.Remove,
                    null,
                    Guid.NewGuid(),
                    null)));

            // Number
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                    new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                        null, 0, false),
                    null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                    new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                        "USD", 2, true),
                    12)));

            // People
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.People));

            // Single Select
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    null, null, null, null, null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    Guid.NewGuid(), null, null, null, null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    null,
                    UpdateCustomFieldSetupRequest.OptionOperation.Add,
                    new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                        "#FF2233", "High"),
                    null,
                    null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    null,
                    UpdateCustomFieldSetupRequest.OptionOperation.Update,
                    new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                        "#FF2233", "High"),
                    Guid.NewGuid(),
                    null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    null,
                    UpdateCustomFieldSetupRequest.OptionOperation.Move,
                    null,
                    Guid.NewGuid(),
                    Guid.NewGuid())));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    null,
                    UpdateCustomFieldSetupRequest.OptionOperation.Remove,
                    null,
                    Guid.NewGuid(),
                    null)));

            // Text
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Text,
                textCustomFieldSetup: new UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Text,
                textCustomFieldSetup: new UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest("Default Text")));

            // Time
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new UpdateCustomFieldSetupRequest.TimeCustomFieldSetupRequest(null)));
            Add(CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new UpdateCustomFieldSetupRequest.TimeCustomFieldSetupRequest(
                    new TimeOnly(10, 55))));
        }
    }
}
