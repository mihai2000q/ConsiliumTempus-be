using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateCustomField;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Api.UnitTests.TestData;

internal static class ProjectTaskControllerData
{
    private static ProjectTaskAggregate CreateWithAssignee()
    {
        var task = ProjectTaskFactory.Create();
        task.UpdateOverview(task.Name, task.Description, UserFactory.Create());
        return task;
    }

    private static ProjectTaskAggregate CreateWithSelectedOptions()
    {
        var task = ProjectTaskFactory.CreateWithCustomFields();
        var multiSelect = task.CustomFields.OfType<MultiSelectCustomField>().Single();
        multiSelect.Setup.Options.ToList().ForEach(multiSelect.AddOption);
        return task;
    }

    internal class GetTasks : TheoryData<ProjectTaskAggregate>
    {
        public GetTasks()
        {
            Add(ProjectTaskFactory.Create());
            Add(CreateWithAssignee());
            Add(ProjectTaskFactory.CreateWithCustomFields());
            Add(CreateWithSelectedOptions());
        }
    }

    internal class GetCollectionTaskResults : TheoryData<GetCollectionProjectTaskResult>
    {
        public GetCollectionTaskResults()
        {
            Add(ProjectTaskResultFactory.CreateGetCollectionProjectTaskResult());
            Add(ProjectTaskResultFactory.CreateGetCollectionProjectTaskResult(
            [
                ProjectTaskFactory.Create(),
                CreateWithAssignee(),
                CreateWithSelectedOptions()
            ]));
        }
    }

    internal class GetUpdateCustomFieldRequests : TheoryData<UpdateCustomFieldFromProjectTaskRequest>
    {
        public GetUpdateCustomFieldRequests()
        {
            // Date
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Date,
                dateCustomField: new UpdateCustomFieldFromProjectTaskRequest.DateCustomFieldRequest(null)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Date,
                dateCustomField: new UpdateCustomFieldFromProjectTaskRequest.DateCustomFieldRequest(
                    new DateOnly(2022, 10, 10))));

            // Date Time
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.DateTime,
                dateTimeCustomField: new UpdateCustomFieldFromProjectTaskRequest.DateTimeCustomFieldRequest(null)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.DateTime,
                dateTimeCustomField: new UpdateCustomFieldFromProjectTaskRequest.DateTimeCustomFieldRequest(
                    new DateTime(2022, 10, 10, 10, 55, 30))));

            // Duration
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Duration,
                durationCustomField: new UpdateCustomFieldFromProjectTaskRequest.DurationCustomFieldRequest(null)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Duration,
                durationCustomField: new UpdateCustomFieldFromProjectTaskRequest.DurationCustomFieldRequest(
                    new TimeSpan(10, 15, 30, 45))));

            // Multi Select
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomField: new UpdateCustomFieldFromProjectTaskRequest.MultiSelectCustomFieldRequest(
                    Guid.NewGuid(), false)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomField: new UpdateCustomFieldFromProjectTaskRequest.MultiSelectCustomFieldRequest(
                    Guid.NewGuid(), true)));

            // Number
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Number,
                numberCustomField: new UpdateCustomFieldFromProjectTaskRequest.NumberCustomFieldRequest(null)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Number,
                numberCustomField: new UpdateCustomFieldFromProjectTaskRequest.NumberCustomFieldRequest(12)));

            // People
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.People,
                peopleCustomField: new UpdateCustomFieldFromProjectTaskRequest.PeopleCustomFieldRequest(null)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.People,
                peopleCustomField: new UpdateCustomFieldFromProjectTaskRequest.PeopleCustomFieldRequest(
                    Guid.NewGuid())));

            // Single Select
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomField: new UpdateCustomFieldFromProjectTaskRequest.SingleSelectCustomFieldRequest(
                    null)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomField: new UpdateCustomFieldFromProjectTaskRequest.SingleSelectCustomFieldRequest(
                    Guid.NewGuid())));

            // Text
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Text,
                textCustomField: new UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest(null)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Text,
                textCustomField: new UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest("Some text")));

            // Time
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Time,
                timeCustomField: new UpdateCustomFieldFromProjectTaskRequest.TimeCustomFieldRequest(null)));
            Add(ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
                type: CustomFieldType.Time,
                timeCustomField: new UpdateCustomFieldFromProjectTaskRequest.TimeCustomFieldRequest(
                    new TimeOnly(10, 55))));
        }
    }
}
