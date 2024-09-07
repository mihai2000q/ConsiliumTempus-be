using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Move;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateCustomField;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateIsCompleted;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static void AssertGetResponse(
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
            IReadOnlyList<ProjectTaskAggregate> tasks,
            int totalCount)
        {
            response.TotalCount.Should().Be(totalCount);
            response.Tasks.Should().HaveCount(tasks.Count);
            response.Tasks
                .Zip(tasks)
                .Should().AllSatisfy(p => AssertResponse(p.First, p.Second));
        }

        internal static void AssertCreation(
            ProjectTaskAggregate task,
            CreateProjectTaskRequest request,
            UserAggregate user,
            List<CustomFieldSetupAggregate> customFieldSetups)
        {
            task.Name.Value.Should().Be(request.Name);
            task.CustomOrderPosition.Value.Should().Be(request.OnTop ? 0 : task.Stage.Tasks.Count - 1);
            task.CreatedBy.Should().Be(user);
            task.Assignee.Should().BeNull();
            task.Stage.Id.Value.Should().Be(request.ProjectStageId);
            task.Stage.Tasks.ShouldBeOrdered();

            task.CustomFields.Should().HaveSameCount(customFieldSetups);
            task.CustomFields.Should().AllSatisfy(customField =>
            {
                switch (customField)
                {
                    case NumberCustomField numberCustomField:
                        var numberSetup = customFieldSetups
                                .SingleOrDefault(s => s == numberCustomField.Setup)
                            as NumberCustomFieldSetupAggregate;
                        numberSetup.Should().NotBeNull();
                        numberCustomField.Number.Should().Be(numberSetup!.DefaultNumber);
                        break;
                    case SingleSelectCustomField singleSelectCustomField:
                        var singleSelectSetup = customFieldSetups
                                .SingleOrDefault(s => s == singleSelectCustomField.Setup)
                            as SingleSelectCustomFieldSetupAggregate;
                        singleSelectSetup.Should().NotBeNull();
                        singleSelectCustomField.Option.Should().Be(singleSelectSetup!.DefaultOption);
                        break;
                    case TextCustomField textCustomField:
                        var textSetup = customFieldSetups
                                .SingleOrDefault(s => s == textCustomField.Setup)
                            as TextCustomFieldSetupAggregate;
                        textSetup.Should().NotBeNull();
                        textCustomField.Text.Should().Be(textSetup!.DefaultText);
                        break;
                }
            });

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertDelete(
            ProjectStage stage,
            DeleteProjectTaskRequest request)
        {
            stage.Tasks.Should().NotContain(s => s.Id.Value == request.Id);
            stage.Tasks.ShouldBeOrdered();

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertMoveWithinStage(
            MoveProjectTaskRequest request,
            ProjectTaskAggregate task,
            int expectedCustomOrderPosition)
        {
            task.Id.Value.Should().Be(request.Id);
            task.CustomOrderPosition.Value.Should().Be(expectedCustomOrderPosition);
            task.Stage.Tasks.ShouldBeOrdered();
            task.Stage.Tasks.Should().ContainSingle(t => t.Id.Value == request.OverId);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertMoveToAnotherStage(
            MoveProjectTaskRequest request,
            ProjectTaskAggregate task,
            List<ProjectStage> stages)
        {
            var overStage = stages.Single(s => s.Id.Value == request.OverId);

            task.Id.Value.Should().Be(request.Id);
            task.CustomOrderPosition.Value.Should().Be(0);
            task.Stage.Should().Be(overStage);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            overStage.Tasks.Should().Contain(task);
            overStage.Tasks[0].Should().Be(task);
            overStage.Tasks.ShouldBeOrdered();

            stages.Should().AllSatisfy(s => s.Tasks.ShouldBeOrdered());
            stages.SelectMany(s => s.Tasks)
                .Should().ContainSingle(t => t == task);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertMoveOverTaskToAnotherStage(
            MoveProjectTaskRequest request,
            ProjectTaskAggregate task,
            ProjectStage overStage,
            List<ProjectStage> stages,
            int expectedCustomOrderPosition)
        {
            overStage.Tasks.Should().ContainSingle(t => t.Id.Value == request.OverId);
            task.Id.Value.Should().Be(request.Id);
            task.CustomOrderPosition.Value.Should().Be(expectedCustomOrderPosition);
            task.Stage.Should().Be(overStage);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            stages.Should().ContainSingle(s => s == overStage);
            stages.Should().AllSatisfy(s => s.Tasks.ShouldBeOrdered());
            stages.SelectMany(s => s.Tasks)
                .Should().ContainSingle(t => t == task);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdated(
            ProjectTaskAggregate newTask,
            ProjectTaskAggregate task,
            UpdateProjectTaskRequest request)
        {
            // unchanged
            newTask.Id.Value.Should().Be(request.Id);
            newTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
            newTask.Stage.Should().Be(task.Stage);

            // changed
            newTask.Name.Value.Should().Be(request.Name);
            newTask.Assignee?.Id.Value.Should().Be(request.AssigneeId!.Value);
            newTask.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            newTask.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newTask.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdateCustomField(
            ProjectTaskAggregate task,
            UpdateCustomFieldFromProjectTaskRequest request,
            UserAggregate? user = null)
        {
            task.Id.Value.Should().Be(request.Id);
            var customField = task.CustomFields.Single(cf => cf.Id.Value == request.CustomFieldId);

            switch (customField)
            {
                case DateCustomField dateCustomField:
                    dateCustomField.Date.Should().Be(request.DateCustomField!.Date);
                    break;

                case DateTimeCustomField dateTimeCustomField:
                    dateTimeCustomField.DateTime.Should().Be(request.DateTimeCustomField!.DateTime);
                    break;

                case DurationCustomField durationCustomField:
                    durationCustomField.Duration.Should().Be(request.DurationCustomField!.Duration);
                    break;
                
                case MultiSelectCustomField multiSelectCustomField:
                    var option = multiSelectCustomField.Setup.Options
                        .Single(o => o.Id == request.MultiSelectCustomField!.OptionId);
                    if (request.MultiSelectCustomField!.Remove)
                        multiSelectCustomField.Options.Should().NotContain(option);
                    else 
                        multiSelectCustomField.Options.Should().Contain(option);
                    break;

                case NumberCustomField numberCustomField:
                    if (request.NumberCustomField!.Number is null)
                        numberCustomField.Number.Should().BeNull();
                    else
                        numberCustomField.Number!.Value.Should().Be(request.NumberCustomField.Number);
                    break;

                case SingleSelectCustomField singleSelectCustomField:
                    var singleOption = singleSelectCustomField.Setup.Options
                        .SingleOrDefault(o => o.Id == request.SingleSelectCustomField!.OptionId);
                    singleSelectCustomField.Option.Should().Be(singleOption);
                    break;
                
                case PeopleCustomField peopleCustomField:
                    if (request.PeopleCustomField!.PersonId is null)
                        peopleCustomField.Person.Should().BeNull();
                    else 
                        peopleCustomField.Person.Should().Be(user);
                    break;

                case TextCustomField textCustomField:
                    if (request.TextCustomField!.Text is null)
                        textCustomField.Text.Should().BeNull();
                    else
                        textCustomField.Text!.Value.Should().Be(request.TextCustomField.Text);
                    break;

                case TimeCustomField timeCustomField:
                    timeCustomField.Time.Should().Be(request.TimeCustomField!.Time);
                    break;
            }
        }

        internal static void AssertUpdatedIsCompleted(
            ProjectTaskAggregate newTask,
            UpdateIsCompletedProjectTaskRequest request)
        {
            // unchanged
            newTask.Id.Value.Should().Be(request.Id);

            // changed
            newTask.IsCompleted.Value.Should().Be(request.IsCompleted);
            if (request.IsCompleted)
                newTask.IsCompleted.CompletedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            else
                newTask.IsCompleted.CompletedOn.Should().BeNull();
            newTask.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            newTask.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newTask.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdatedOverview(
            ProjectTaskAggregate newTask,
            ProjectTaskAggregate task,
            UpdateOverviewProjectTaskRequest request)
        {
            // unchanged
            newTask.Id.Value.Should().Be(request.Id);
            newTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
            newTask.Stage.Should().Be(task.Stage);

            // changed
            newTask.Name.Value.Should().Be(request.Name);
            newTask.Description.Value.Should().Be(request.Description);
            if (request.AssigneeId is null)
                newTask.Assignee.Should().BeNull();
            else
                newTask.Assignee!.Id.Value.Should().Be(request.AssigneeId.Value);
            newTask.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            newTask.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newTask.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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
            response.Stages
                .Zip(sprint.Stages.OrderBy(s => s.CustomOrderPosition))
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

        private static void AssertResponse(
            GetCollectionProjectTaskResponse.ProjectTaskResponse response,
            ProjectTaskAggregate projectTask)
        {
            response.Id.Should().Be(projectTask.Id.Value);
            response.Name.Should().Be(projectTask.Name.Value);
            response.IsCompleted.Should().Be(projectTask.IsCompleted.Value);
            AssertUserResponse(response.Assignee, projectTask.Assignee);
            response.CustomFields
                .Zip(projectTask.CustomFields.OrderBy(cf => cf.Setup.Audit.CreatedDateTime))
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