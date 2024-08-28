using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Move;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateCustomField;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateIsCompleted;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static bool AssertGetProjectTaskQuery(
            GetProjectTaskQuery query,
            GetProjectTaskRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertGetCollectionProjectTaskQuery(
            GetCollectionProjectTaskQuery query,
            GetCollectionProjectTaskRequest request)
        {
            query.ProjectStageId.Should().Be(request.ProjectStageId);
            query.Search.Should().BeEquivalentTo(request.Search);
            query.OrderBy.Should().BeEquivalentTo(request.OrderBy);
            query.CurrentPage.Should().Be(request.CurrentPage);
            query.PageSize.Should().Be(request.PageSize);

            return true;
        }

        internal static bool AssertCreateCommand(
            CreateProjectTaskCommand command,
            CreateProjectTaskRequest request)
        {
            command.ProjectStageId.Should().Be(request.ProjectStageId);
            command.Name.Should().Be(request.Name);
            command.OnTop.Should().Be(request.OnTop);

            return true;
        }

        internal static bool AssertMoveCommand(
            MoveProjectTaskCommand command,
            MoveProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.OverId.Should().Be(request.OverId);

            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateProjectTaskCommand command,
            UpdateProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.AssigneeId.Should().Be(request.AssigneeId);

            return true;
        }

        internal static bool AssertUpdateCustomFieldCommand(
            UpdateCustomFieldFromProjectTaskCommand command,
            UpdateCustomFieldFromProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.CustomFieldId.Should().Be(request.CustomFieldId);
            command.Type.Should().Be(request.Type);
            AssertUpdateNumberCustomFieldRequest(request.NumberCustomField, command.NumberCustomField);
            AssertUpdateSingleSelectCustomFieldRequest(request.SingleSelectCustomField, command.SingleSelectCustomField);
            AssertUpdateTextCustomFieldRequest(request.TextCustomField, command.TextCustomField);

            return true;
        }

        internal static bool AssertUpdateIsCompletedCommand(
            UpdateIsCompletedProjectTaskCommand command,
            UpdateIsCompletedProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.IsCompleted.Should().Be(request.IsCompleted);

            return true;
        }

        internal static bool AssertUpdateOverviewCommand(
            UpdateOverviewProjectTaskCommand command,
            UpdateOverviewProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.AssigneeId.Should().Be(request.AssigneeId);

            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteProjectTaskCommand command,
            DeleteProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.StageId.Should().Be(request.StageId);

            return true;
        }

        internal static void AssertGetProjectTaskResponse(
            GetProjectTaskResponse response,
            ProjectTaskAggregate task)
        {
            response.Name.Should().Be(task.Name.Value);
            response.Description.Should().Be(task.Description.Value);
            response.IsCompleted.Should().Be(task.IsCompleted.Value);
            AssertUserResponse(response.Assignee, task.Assignee);
            AssertProjectStageResponse(response.Stage, task.Stage);
            AssertProjectSprintResponse(response.Sprint, task.Stage.Sprint);
            AssertProjectResponse(response.Project, task.Stage.Sprint.Project);
            AssertWorkspaceResponse(response.Workspace, task.Stage.Sprint.Project.Workspace);
            response.CustomFields
                .Zip(task.CustomFields.OrderBy(cf => cf.Setup.Audit.CreatedDateTime))
                .Should().AllSatisfy(x => AssertCustomFieldResponse(x.First, x.Second));
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectTaskResponse response,
            GetCollectionProjectTaskResult result)
        {
            response.Tasks.Zip(result.Tasks)
                .Should().AllSatisfy(p => AssertProjectTaskResponse(p.First, p.Second));
            response.TotalCount.Should().Be(result.TotalCount);
        }
        
        private static void AssertUpdateNumberCustomFieldRequest(
            UpdateCustomFieldFromProjectTaskRequest.NumberCustomFieldRequest? request,
            UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand? command)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command!.Number.Should().Be(request.Number);
        }

        private static void AssertUpdateSingleSelectCustomFieldRequest(
            UpdateCustomFieldFromProjectTaskRequest.SingleSelectCustomFieldRequest? request,
            UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand? command)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command!.OptionId.Should().Be(request.OptionId);
        }

        private static void AssertUpdateTextCustomFieldRequest(
            UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest? request,
            UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand? command)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command!.Text.Should().Be(request.Text);
        }

        private static void AssertCustomFieldResponse(
            GetProjectTaskResponse.CustomFieldResponse response,
            CustomField customField)
        {
            response.Id.Should().Be(customField.Id.Value);
            response.Name.Should().Be(customField.Setup.Name.Value);
            response.Description.Should().Be(customField.Setup.Description.Value);

            switch (response)
            {
                case GetProjectTaskResponse.DateCustomFieldResponse dateResponse:
                    customField.Should().BeOfType<DateCustomField>();
                    AssertDateCustomFieldResponse(dateResponse, (DateCustomField)customField);
                    break;

                case GetProjectTaskResponse.DateTimeCustomFieldResponse dateTimeResponse:
                    customField.Should().BeOfType<DateTimeCustomField>();
                    AssertDateTimeCustomFieldResponse(dateTimeResponse, (DateTimeCustomField)customField);
                    break;

                case GetProjectTaskResponse.DurationCustomFieldResponse durationResponse:
                    customField.Should().BeOfType<DurationCustomField>();
                    AssertDurationCustomFieldResponse(durationResponse, (DurationCustomField)customField);
                    break;

                case GetProjectTaskResponse.MultiSelectCustomFieldResponse multiSelectResponse:
                    customField.Should().BeOfType<MultiSelectCustomField>();
                    AssertMultiSelectCustomFieldResponse(multiSelectResponse, (MultiSelectCustomField)customField);
                    break;

                case GetProjectTaskResponse.NumberCustomFieldResponse numberResponse:
                    customField.Should().BeOfType<NumberCustomField>();
                    AssertNumberCustomFieldResponse(numberResponse, (NumberCustomField)customField);
                    break;

                case GetProjectTaskResponse.PeopleCustomFieldResponse peopleResponse:
                    customField.Should().BeOfType<PeopleCustomField>();
                    AssertPeopleCustomFieldResponse(peopleResponse, (PeopleCustomField)customField);
                    break;

                case GetProjectTaskResponse.SingleSelectCustomFieldResponse singleSelectResponse:
                    customField.Should().BeOfType<SingleSelectCustomField>();
                    AssertSingleSelectCustomFieldResponse(singleSelectResponse, (SingleSelectCustomField)customField);
                    break;

                case GetProjectTaskResponse.TextCustomFieldResponse textResponse:
                    customField.Should().BeOfType<TextCustomField>();
                    AssertTextCustomFieldResponse(textResponse, (TextCustomField)customField);
                    break;

                case GetProjectTaskResponse.TimeCustomFieldResponse timeResponse:
                    customField.Should().BeOfType<TimeCustomField>();
                    AssertTimeCustomFieldResponse(timeResponse, (TimeCustomField)customField);
                    break;
            }
        }

        private static void AssertDateCustomFieldResponse(
            GetProjectTaskResponse.DateCustomFieldResponse response,
            DateCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Date);
            response.Date.Should().Be(customField.Date);
        }

        private static void AssertDateTimeCustomFieldResponse(
            GetProjectTaskResponse.DateTimeCustomFieldResponse response,
            DateTimeCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.DateTime);
            response.DateTime.Should().Be(customField.DateTime);
        }

        private static void AssertDurationCustomFieldResponse(
            GetProjectTaskResponse.DurationCustomFieldResponse response,
            DurationCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Duration);
            response.Duration.Should().Be(customField.Duration);
        }

        private static void AssertMultiSelectCustomFieldResponse(
            GetProjectTaskResponse.MultiSelectCustomFieldResponse response,
            MultiSelectCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.MultiSelect);
            response.Options
                .Zip(customField.Options)
                .Should().AllSatisfy(x => AssertMultiSelectOptionResponse(x.First, x.Second));
            response.AvailableOptions
                .Zip(customField.Setup.Options)
                .Should().AllSatisfy(x => AssertMultiSelectOptionResponse(x.First, x.Second));
        }

        private static void AssertNumberCustomFieldResponse(
            GetProjectTaskResponse.NumberCustomFieldResponse response,
            NumberCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Number);
            if (customField.Number is null)
                response.Number.Should().BeNull();
            else
                response.Number.Should().Be(customField.Number.Value);
        }

        private static void AssertPeopleCustomFieldResponse(
            GetProjectTaskResponse.PeopleCustomFieldResponse response,
            PeopleCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.People);
            AssertUserResponse(response.Person, customField.Person);
        }

        private static void AssertSingleSelectCustomFieldResponse(
            GetProjectTaskResponse.SingleSelectCustomFieldResponse response,
            SingleSelectCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.SingleSelect);
            AssertSingleSelectOptionResponse(response.Option, customField.Option);
            response.AvailableOptions
                .Zip(customField.Setup.Options)
                .Should().AllSatisfy(x => AssertSingleSelectOptionResponse(x.First, x.Second));
        }

        private static void AssertTextCustomFieldResponse(
            GetProjectTaskResponse.TextCustomFieldResponse response,
            TextCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Text);
            if (customField.Text is null)
                response.Text.Should().BeNull();
            else
                response.Text.Should().Be(customField.Text.Value);
        }

        private static void AssertTimeCustomFieldResponse(
            GetProjectTaskResponse.TimeCustomFieldResponse response,
            TimeCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Time);
            response.Time.Should().Be(customField.Time);
        }

        private static void AssertMultiSelectOptionResponse(
            GetProjectTaskResponse.MultiSelectCustomFieldResponse.MultiSelectOptionResponse response,
            MultiSelectOption multiSelectOption)
        {
            response.Id.Should().Be(multiSelectOption.Id);
            response.Value.Should().Be(multiSelectOption.Value);
            response.Color.Should().Be(multiSelectOption.Color);
        }

        private static void AssertSingleSelectOptionResponse(
            GetProjectTaskResponse.SingleSelectCustomFieldResponse.SingleSelectOptionResponse? response,
            SingleSelectOption? singleSelectOption)
        {
            if (singleSelectOption is null)
            {
                response.Should().BeNull();
                return;
            }

            response!.Id.Should().Be(singleSelectOption.Id);
            response.Value.Should().Be(singleSelectOption.Value);
            response.Color.Should().Be(singleSelectOption.Color);
        }

        private static void AssertUserResponse(
            GetProjectTaskResponse.UserResponse? response,
            UserAggregate? user)
        {
            if (user is null)
            {
                response.Should().BeNull();
                return;
            }

            response!.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.Name.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertProjectStageResponse(
            GetProjectTaskResponse.ProjectStageResponse response,
            ProjectStage stage)
        {
            response.Id.Should().Be(stage.Id.Value);
            response.Name.Should().Be(stage.Name.Value);
        }

        private static void AssertProjectSprintResponse(
            GetProjectTaskResponse.ProjectSprintResponse response,
            ProjectSprintAggregate sprint)
        {
            response.Id.Should().Be(sprint.Id.Value);
            response.Name.Should().Be(sprint.Name.Value);
            response.Stages.Zip(sprint.Stages)
                .Should().AllSatisfy(x => AssertProjectStageResponse(x.First, x.Second));
        }

        private static void AssertProjectResponse(
            GetProjectTaskResponse.ProjectResponse response,
            ProjectAggregate project)
        {
            response.Id.Should().Be(project.Id.Value);
            response.Name.Should().Be(project.Name.Value);
        }

        private static void AssertWorkspaceResponse(
            GetProjectTaskResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Id.Should().Be(workspace.Id.Value);
            response.Name.Should().Be(workspace.Name.Value);
        }

        private static void AssertProjectTaskResponse(
            GetCollectionProjectTaskResponse.ProjectTaskResponse taskResponse,
            ProjectTaskAggregate task)
        {
            taskResponse.Id.Should().Be(task.Id.Value);
            taskResponse.Name.Should().Be(task.Name.Value);
            taskResponse.Description.Should().Be(task.Description.Value);
            taskResponse.IsCompleted.Should().Be(task.IsCompleted.Value);
            AssertUserResponse(taskResponse.Assignee, task.Assignee);
            taskResponse.CustomFields
                .Zip(task.CustomFields)
                .Should().AllSatisfy(x => AssertCustomFieldResponse(x.First, x.Second));
        }

        private static void AssertUserResponse(
            GetCollectionProjectTaskResponse.UserResponse? response,
            UserAggregate? user)
        {
            if (user is null)
            {
                response.Should().BeNull();
                return;
            }

            response!.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.Name.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertCustomFieldResponse(
            GetCollectionProjectTaskResponse.CustomFieldResponse response,
            CustomField customField)
        {
            response.Id.Should().Be(customField.Id.Value);
            response.Name.Should().Be(customField.Setup.Name.Value);
            response.Description.Should().Be(customField.Setup.Description.Value);

            switch (response)
            {
                case GetCollectionProjectTaskResponse.DateCustomFieldResponse dateResponse:
                    customField.Should().BeOfType<DateCustomField>();
                    AssertDateCustomFieldResponse(dateResponse, (DateCustomField)customField);
                    break;

                case GetCollectionProjectTaskResponse.DateTimeCustomFieldResponse dateTimeResponse:
                    customField.Should().BeOfType<DateTimeCustomField>();
                    AssertDateTimeCustomFieldResponse(dateTimeResponse, (DateTimeCustomField)customField);
                    break;

                case GetCollectionProjectTaskResponse.DurationCustomFieldResponse durationResponse:
                    customField.Should().BeOfType<DurationCustomField>();
                    AssertDurationCustomFieldResponse(durationResponse, (DurationCustomField)customField);
                    break;

                case GetCollectionProjectTaskResponse.MultiSelectCustomFieldResponse multiSelectResponse:
                    customField.Should().BeOfType<MultiSelectCustomField>();
                    AssertMultiSelectCustomFieldResponse(multiSelectResponse, (MultiSelectCustomField)customField);
                    break;

                case GetCollectionProjectTaskResponse.NumberCustomFieldResponse numberResponse:
                    customField.Should().BeOfType<NumberCustomField>();
                    AssertNumberCustomFieldResponse(numberResponse, (NumberCustomField)customField);
                    break;

                case GetCollectionProjectTaskResponse.PeopleCustomFieldResponse peopleResponse:
                    customField.Should().BeOfType<PeopleCustomField>();
                    AssertPeopleCustomFieldResponse(peopleResponse, (PeopleCustomField)customField);
                    break;

                case GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse singleSelectResponse:
                    customField.Should().BeOfType<SingleSelectCustomField>();
                    AssertSingleSelectCustomFieldResponse(singleSelectResponse, (SingleSelectCustomField)customField);
                    break;

                case GetCollectionProjectTaskResponse.TextCustomFieldResponse textResponse:
                    customField.Should().BeOfType<TextCustomField>();
                    AssertTextCustomFieldResponse(textResponse, (TextCustomField)customField);
                    break;

                case GetCollectionProjectTaskResponse.TimeCustomFieldResponse timeResponse:
                    customField.Should().BeOfType<TimeCustomField>();
                    AssertTimeCustomFieldResponse(timeResponse, (TimeCustomField)customField);
                    break;
            }
        }

        private static void AssertDateCustomFieldResponse(
            GetCollectionProjectTaskResponse.DateCustomFieldResponse response,
            DateCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Date);
            response.Date.Should().Be(customField.Date);
        }

        private static void AssertDateTimeCustomFieldResponse(
            GetCollectionProjectTaskResponse.DateTimeCustomFieldResponse response,
            DateTimeCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.DateTime);
            response.DateTime.Should().Be(customField.DateTime);
        }

        private static void AssertDurationCustomFieldResponse(
            GetCollectionProjectTaskResponse.DurationCustomFieldResponse response,
            DurationCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Duration);
            response.Duration.Should().Be(customField.Duration);
        }

        private static void AssertMultiSelectCustomFieldResponse(
            GetCollectionProjectTaskResponse.MultiSelectCustomFieldResponse response,
            MultiSelectCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.MultiSelect);
            response.Options
                .Zip(customField.Options)
                .Should().AllSatisfy(x => AssertMultiSelectOptionResponse(x.First, x.Second));
        }

        private static void AssertNumberCustomFieldResponse(
            GetCollectionProjectTaskResponse.NumberCustomFieldResponse response,
            NumberCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Number);
            if (customField.Number is null)
                response.Number.Should().BeNull();
            else
                response.Number.Should().Be(customField.Number.Value);
        }

        private static void AssertPeopleCustomFieldResponse(
            GetCollectionProjectTaskResponse.PeopleCustomFieldResponse response,
            PeopleCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.People);
            AssertUserResponse(response.Person, customField.Person);
        }

        private static void AssertSingleSelectCustomFieldResponse(
            GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse response,
            SingleSelectCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.SingleSelect);
            AssertSingleSelectOptionResponse(response.Option, customField.Option);
        }

        private static void AssertTextCustomFieldResponse(
            GetCollectionProjectTaskResponse.TextCustomFieldResponse response,
            TextCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Text);
            if (customField.Text is null)
                response.Text.Should().BeNull();
            else
                response.Text.Should().Be(customField.Text.Value);
        }

        private static void AssertTimeCustomFieldResponse(
            GetCollectionProjectTaskResponse.TimeCustomFieldResponse response,
            TimeCustomField customField)
        {
            response.Type.Should().Be(CustomFieldType.Time);
            response.Time.Should().Be(customField.Time);
        }

        private static void AssertMultiSelectOptionResponse(
            GetCollectionProjectTaskResponse.MultiSelectCustomFieldResponse.MultiSelectOptionResponse response,
            MultiSelectOption multiSelectOption)
        {
            response.Id.Should().Be(multiSelectOption.Id);
            response.Value.Should().Be(multiSelectOption.Value);
            response.Color.Should().Be(multiSelectOption.Color);
        }

        private static void AssertSingleSelectOptionResponse(
            GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse.SingleSelectOptionResponse? response,
            SingleSelectOption? singleSelectOption)
        {
            if (singleSelectOption is null)
            {
                response.Should().BeNull();
                return;
            }

            response!.Id.Should().Be(singleSelectOption.Id);
            response.Value.Should().Be(singleSelectOption.Value);
            response.Color.Should().Be(singleSelectOption.Color);
        }
    }
}