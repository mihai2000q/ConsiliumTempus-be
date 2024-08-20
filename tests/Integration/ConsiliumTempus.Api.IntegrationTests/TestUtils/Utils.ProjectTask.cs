using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Move;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
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

        private static void AssertResponse(
            GetCollectionProjectTaskResponse.ProjectTaskResponse response,
            ProjectTaskAggregate projectTask)
        {
            response.Id.Should().Be(projectTask.Id.Value);
            response.Name.Should().Be(projectTask.Name.Value);
            response.IsCompleted.Should().Be(projectTask.IsCompleted.Value);
            AssertUserResponse(response.Assignee, projectTask.Assignee);
            response.CustomFields.OrderBy(cf => cf.Id)
                .Zip(projectTask.CustomFields.OrderBy(cf => cf.Id.Value))
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

            switch (response)
            {
                case GetCollectionProjectTaskResponse.NumberCustomFieldResponse numberResponse:
                    AssertNumberCustomFieldResponse(numberResponse, customField);
                    break;

                case GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse singleSelectResponse:
                    AssertSingleSelectCustomFieldResponse(singleSelectResponse, customField);
                    break;

                case GetCollectionProjectTaskResponse.TextCustomFieldResponse textResponse:
                    AssertTextCustomFieldResponse(textResponse, customField);
                    break;
            }
        }

        private static void AssertNumberCustomFieldResponse(
            GetCollectionProjectTaskResponse.NumberCustomFieldResponse response,
            CustomField customField)
        {
            customField.Should().BeOfType<NumberCustomField>();
            var numberCustomField = (NumberCustomField)customField;
            response.Type.Should().Be(CustomFieldType.Number);
            response.Name.Should().Be(numberCustomField.Setup.Name.Value);
            response.Description.Should().Be(numberCustomField.Setup.Description.Value);
            response.Description.Should().Be(numberCustomField.Setup.Description.Value);
            if (numberCustomField.Number is null)
                response.Number.Should().BeNull();
            else
                response.Number.Should().Be(numberCustomField.Number.Value);
        }

        private static void AssertSingleSelectCustomFieldResponse(
            GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse response,
            CustomField customField)
        {
            customField.Should().BeOfType<SingleSelectCustomField>();
            var singleSelectCustomField = (SingleSelectCustomField)customField;
            response.Type.Should().Be(CustomFieldType.SingleSelect);
            response.Name.Should().Be(singleSelectCustomField.Setup.Name.Value);
            response.Description.Should().Be(singleSelectCustomField.Setup.Description.Value);
            response.Description.Should().Be(singleSelectCustomField.Setup.Description.Value);
            AssertSingleSelectOptionResponse(response.Option, singleSelectCustomField.Option);
            response.AvailableOptions
                .Zip(singleSelectCustomField.Setup.Options)
                .Should().AllSatisfy(x => AssertSingleSelectOptionResponse(x.First, x.Second));
        }

        private static void AssertTextCustomFieldResponse(
            GetCollectionProjectTaskResponse.TextCustomFieldResponse response,
            CustomField customField)
        {
            customField.Should().BeOfType<TextCustomField>();
            var textCustomField = (TextCustomField)customField;
            response.Type.Should().Be(CustomFieldType.Text);
            response.Name.Should().Be(textCustomField.Setup.Name.Value);
            response.Description.Should().Be(textCustomField.Setup.Description.Value);
            response.Description.Should().Be(textCustomField.Setup.Description.Value);
            if (textCustomField.Text is null)
                response.Text.Should().BeNull();
            else
                response.Text.Should().Be(textCustomField.Text.Value);
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