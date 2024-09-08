using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.Events;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static void AssertFromCreateCommand(
            CreateProjectTaskCommand command,
            ProjectStage stage,
            UserAggregate user)
        {
            var task = command.OnTop ? stage.Tasks[0] : stage.Tasks[^1];

            task.Id.Value.Should().NotBeEmpty();
            task.Name.Value.Should().Be(command.Name);
            task.Description.Value.Should().Be(string.Empty);
            task.CustomOrderPosition.Value.Should().Be(command.OnTop ? 0 : stage.Tasks.Count - 1);
            task.IsCompleted.Value.Should().Be(false);
            task.CreatedBy.Should().Be(user);
            task.Assignee.Should().BeNull();
            task.Reviewer.Should().BeNull();
            task.DueDate.Should().BeNull();
            task.EstimatedDuration.Should().BeNull();
            task.Stage.Should().Be(stage);
            task.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Comments.Should().BeEmpty();
            task.DomainEvents.Should().HaveCount(1);
            var domainEvent = task.DomainEvents[0];
            domainEvent.Should().BeOfType<ProjectTaskCreated>();
            ((ProjectTaskCreated)domainEvent).ProjectTask.Should().Be(task);

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            var count = 0;
            stage.Tasks.Should().AllSatisfy(t => t.CustomOrderPosition.Value.Should().Be(count++));
        }

        internal static void AssertFromDeleteCommand(
            ProjectTaskAggregate task,
            DeleteProjectTaskCommand command)
        {
            task.Id.Value.Should().Be(command.Id);
            task.DomainEvents.Should().HaveCount(1);
            var domainEvent = task.DomainEvents[0];
            domainEvent.Should().BeOfType<ProjectTaskDeleted>();
            ((ProjectTaskDeleted)domainEvent).ProjectTask.Should().Be(task);

            var stage = task.Stage;

            stage.Tasks.Should().NotContain(task);
            stage.Tasks.ShouldBeOrdered();

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromMoveCommandToAnotherStage(
            ProjectTaskAggregate task,
            MoveProjectTaskCommand command)
        {
            var overStage = task.Stage.Sprint.Stages.Single(s => s.Id.Value == command.OverId);

            task.Id.Value.Should().Be(command.Id);
            task.CustomOrderPosition.Value.Should().Be(0);
            task.Stage.Should().Be(overStage);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            overStage.Tasks.Should().ContainSingle(t => t == task);
            overStage.Tasks[0].Should().Be(task);
            overStage.Tasks.ShouldBeOrdered();

            task.Stage.Sprint.Stages.Should().AllSatisfy(s => s.Tasks.ShouldBeOrdered());
            task.Stage.Sprint.Stages.SelectMany(s => s.Tasks)
                .Should().ContainSingle(t => t == task);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromMoveCommandWithinStage(
            ProjectTaskAggregate task,
            MoveProjectTaskCommand command,
            int expectedCustomOrderPosition)
        {
            task.Id.Value.Should().Be(command.Id);
            task.CustomOrderPosition.Value.Should().Be(expectedCustomOrderPosition);
            task.Stage.Tasks.ShouldBeOrdered();
            task.Stage.Tasks.Should().ContainSingle(t => t.Id.Value == command.OverId);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromMoveCommandOverTaskToAnotherStage(
            ProjectTaskAggregate task,
            MoveProjectTaskCommand command,
            int expectedCustomOrderPosition)
        {
            var overStage = task.Stage.Sprint.Stages
                .SelectMany(s => s.Tasks)
                .Single(s => s.Id.Value == command.OverId)
                .Stage;

            task.Id.Value.Should().Be(command.Id);
            task.CustomOrderPosition.Value.Should().Be(expectedCustomOrderPosition);
            task.Stage.Should().Be(overStage);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Stages.Should().AllSatisfy(s => s.Tasks.ShouldBeOrdered());
            task.Stage.Sprint.Stages.SelectMany(s => s.Tasks)
                .Should().ContainSingle(t => t == task);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateCommand(
            ProjectTaskAggregate task,
            UpdateProjectTaskCommand command,
            UserAggregate assignee)
        {
            task.Name.Value.Should().Be(command.Name);
            task.Assignee.Should().Be(command.AssigneeId is null ? null : assignee);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateCustomFieldCommand(
            ProjectTaskAggregate task,
            UpdateCustomFieldFromProjectTaskCommand command,
            UserAggregate user)
        {
            task.Id.Value.Should().Be(command.Id);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            var customField = task.CustomFields.SingleOrDefault(cf => cf.Id.Value == command.CustomFieldId);
            customField.Should().NotBeNull();

            switch (customField)
            {
                case DateCustomField dateCustomField:
                    dateCustomField.Date.Should().Be(command.DateCustomField!.Date);
                    break;

                case DateTimeCustomField dateTimeCustomField:
                    dateTimeCustomField.DateTime.Should().Be(command.DateTimeCustomField!.DateTime);
                    break;

                case DurationCustomField durationCustomField:
                    durationCustomField.Duration.Should().Be(command.DurationCustomField!.Duration);
                    break;
                
                case MultiSelectCustomField multiSelectCustomField:
                    var option = multiSelectCustomField.Setup.Options
                        .Single(o => o.Id == command.MultiSelectCustomField!.OptionId);
                    if (command.MultiSelectCustomField!.Remove)
                        multiSelectCustomField.Options.Should().NotContain(option);
                    else 
                        multiSelectCustomField.Options.Should().Contain(option);
                    break;

                case NumberCustomField numberCustomField:
                    if (command.NumberCustomField!.Number is null)
                        numberCustomField.Number.Should().BeNull();
                    else
                        numberCustomField.Number!.Value.Should().Be(command.NumberCustomField.Number);
                    break;

                case SingleSelectCustomField singleSelectCustomField:
                    var singleOption = singleSelectCustomField.Setup.Options
                        .SingleOrDefault(o => o.Id == command.SingleSelectCustomField!.OptionId);
                    singleSelectCustomField.Option.Should().Be(singleOption);
                    break;
                
                case PeopleCustomField peopleCustomField:
                    if (command.PeopleCustomField!.PersonId is null)
                        peopleCustomField.Person.Should().BeNull();
                    else 
                        peopleCustomField.Person.Should().Be(user);
                    break;

                case TextCustomField textCustomField:
                    if (command.TextCustomField!.Text is null)
                        textCustomField.Text.Should().BeNull();
                    else
                        textCustomField.Text!.Value.Should().Be(command.TextCustomField.Text);
                    break;

                case TimeCustomField timeCustomField:
                    timeCustomField.Time.Should().Be(command.TimeCustomField!.Time);
                    break;
            }

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateIsCompletedCommand(
            ProjectTaskAggregate task,
            UpdateIsCompletedProjectTaskCommand command)
        {
            task.IsCompleted.Value.Should().Be(command.IsCompleted);
            if (command.IsCompleted)
                task.IsCompleted.CompletedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            else
                task.IsCompleted.CompletedOn.Should().BeNull();
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateOverviewCommand(
            ProjectTaskAggregate task,
            UpdateOverviewProjectTaskCommand command,
            UserAggregate assignee)
        {
            task.Name.Value.Should().Be(command.Name);
            task.Description.Value.Should().Be(command.Description);
            task.Assignee.Should().Be(command.AssigneeId is null ? null : assignee);
            task.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromProjectTaskCreated(
            ProjectTaskCreated domainEvent,
            List<CustomFieldSetupAggregate> setups)
        {
            domainEvent.ProjectTask.CustomFields.Should().HaveSameCount(setups);
            domainEvent.ProjectTask.CustomFields
                .Zip(setups)
                .Should().AllSatisfy(x =>
                {
                    var (customField, setup) = x;
                    customField.Id.Value.Should().NotBeEmpty();
                    customField.ProjectTask.Should().Be(domainEvent.ProjectTask);

                    switch (customField)
                    {
                        case NumberCustomField numberCustomField:
                            setup.Should().BeOfType<NumberCustomFieldSetupAggregate>();
                            numberCustomField.Number
                                .Should().Be(((NumberCustomFieldSetupAggregate)setup).DefaultNumber);
                            numberCustomField.Setup.Should().Be(setup);
                            break;
                        case SingleSelectCustomField singleSelectCustomField:
                            setup.Should().BeOfType<SingleSelectCustomFieldSetupAggregate>();
                            singleSelectCustomField.Option
                                .Should().Be(((SingleSelectCustomFieldSetupAggregate)setup).DefaultOption);
                            singleSelectCustomField.Setup.Should().Be(setup);
                            break;
                        case TextCustomField textCustomField:
                            setup.Should().BeOfType<TextCustomFieldSetupAggregate>();
                            textCustomField.Text.Should().Be(((TextCustomFieldSetupAggregate)setup).DefaultText);
                            textCustomField.Setup.Should().Be(setup);
                            break;
                    }
                });
        }

        internal static void AssertProjectTask(
            ProjectTaskAggregate outcome,
            ProjectTaskAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.Description.Should().Be(expected.Description);
            outcome.CustomOrderPosition.Should().Be(expected.CustomOrderPosition);
            outcome.IsCompleted.Should().Be(expected.IsCompleted);
            outcome.CreatedBy.Should().Be(expected.CreatedBy);
            outcome.Assignee.Should().Be(expected.Assignee);
            outcome.Reviewer.Should().Be(expected.Reviewer);
            outcome.DueDate.Should().Be(expected.DueDate);
            outcome.EstimatedDuration.Should().Be(expected.EstimatedDuration);
            outcome.CreatedDateTime.Should().Be(expected.CreatedDateTime);
            outcome.UpdatedDateTime.Should().Be(expected.UpdatedDateTime);
            outcome.Stage.Should().Be(expected.Stage);
            outcome.Comments.Should().BeEquivalentTo(expected.Comments);
        }
    }
}