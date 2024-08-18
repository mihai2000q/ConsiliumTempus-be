using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class CustomFieldSetup
    {
        internal static void AssertAddToProjectCommand(
            AddCustomFieldSetupToProjectCommand command,
            CustomFieldSetupAggregate customFieldSetup,
            ProjectAggregate project)
        {
            project.Id.Value.Should().Be(command.ProjectId);
            customFieldSetup.Id.Value.Should().Be(command.Id);
            customFieldSetup.Projects.Should().Contain(project);
            customFieldSetup.DomainEvents.Should().HaveCount(1);
            var domainEvent = customFieldSetup.DomainEvents[0];
            domainEvent.Should().BeOfType<AddedCustomFieldSetupToProject>();
            ((AddedCustomFieldSetupToProject)domainEvent).CustomFieldSetup.Should().Be(customFieldSetup);
            ((AddedCustomFieldSetupToProject)domainEvent).Project.Should().Be(project);
            
            customFieldSetup.Workspace!.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
        }
        
        internal static void AssertFromCreateCommand(
            CreateCustomFieldSetupCommand command,
            CustomFieldSetupAggregate customFieldSetup,
            UserAggregate user,
            WorkspaceAggregate? workspace,
            ProjectAggregate? project)
        {
            customFieldSetup.Id.Value.Should().NotBeEmpty();
            customFieldSetup.Name.Value.Should().Be(command.Name);
            customFieldSetup.Description.Value.Should().Be(command.Description);
            customFieldSetup.Audit.ShouldBeCreated(user);

            if (command.WorkspaceId is not null)
                customFieldSetup.Workspace.Should().Be(workspace);
            else
                customFieldSetup.Workspace.Should().BeNull();

            if (command.ProjectId is not null)
            {
                project.Should().NotBeNull();
                customFieldSetup.Projects.Should().Contain(project!);
                customFieldSetup.DomainEvents.Should().HaveCount(1);
                var domainEvent = customFieldSetup.DomainEvents[0];
                domainEvent.Should().BeOfType<AddedCustomFieldSetupToProject>();
                ((AddedCustomFieldSetupToProject)domainEvent).CustomFieldSetup.Should().Be(customFieldSetup);
                ((AddedCustomFieldSetupToProject)domainEvent).Project.Should().Be(project);
            }

            var customFieldType = Enum.Parse<CustomFieldType>(command.Type);
            switch (customFieldType)
            {
                case CustomFieldType.Number:
                    AssertNumberCustomFieldSetup(customFieldSetup, command);
                    break;
                case CustomFieldType.SingleSelect:
                    AssertSingleSelectCustomFieldSetup(customFieldSetup, command);
                    break;
                case CustomFieldType.Text:
                    AssertTextCustomFieldSetup(customFieldSetup, command);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command));
            }

            workspace?.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project?.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project?.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromCustomFieldSetupCreated(
            AddedCustomFieldSetupToProject domainEvent,
            List<ProjectTaskAggregate> tasks)
        {
            var (setup, _) = domainEvent;
            tasks.Should().AllSatisfy(task =>
            {
                task.CustomFields.Should().HaveCount(1);
                var customField = task.CustomFields[0];

                customField.Id.Value.Should().NotBeEmpty();
                customField.ProjectTask.Should().Be(task);
                switch (setup)
                {
                    case NumberCustomFieldSetupAggregate numberSetup:
                        customField.Should().BeOfType<NumberCustomField>();
                        ((NumberCustomField)customField).Number.Should().Be(numberSetup.DefaultNumber);
                        ((NumberCustomField)customField).Setup.Should().Be(setup);
                        break;
                    case SingleSelectCustomFieldSetupAggregate singleSelectSetup:
                        customField.Should().BeOfType<SingleSelectCustomField>();
                        ((SingleSelectCustomField)customField).Option.Should().Be(singleSelectSetup.DefaultOption);
                        ((SingleSelectCustomField)customField).Setup.Should().Be(setup);
                        break;
                    case TextCustomFieldSetupAggregate textSetup:
                        customField.Should().BeOfType<TextCustomField>();
                        ((TextCustomField)customField).Text.Should().Be(textSetup.DefaultText);
                        ((TextCustomField)customField).Setup.Should().Be(setup);
                        break;
                }
            });
        }

        private static void AssertNumberCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupCommand command)
        {
            customFieldSetup.Should().BeOfType<NumberCustomFieldSetupAggregate>();
            var setup = (NumberCustomFieldSetupAggregate)customFieldSetup;
            setup.Settings.CurrencyCode.Should().Be(command.NumberCustomFieldSetup!.Settings.CurrencyCode);
            setup.Settings.Decimals.Should().Be((short)command.NumberCustomFieldSetup!.Settings.Decimals);
            setup.Settings.Rounding.Should().Be(command.NumberCustomFieldSetup!.Settings.Rounding);
            if (command.NumberCustomFieldSetup.DefaultNumber is null)
                setup.DefaultNumber.Should().BeNull();
            else
                setup.DefaultNumber!.Value.Should().Be(command.NumberCustomFieldSetup.DefaultNumber);
        }

        private static void AssertSingleSelectCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupCommand command)
        {
            customFieldSetup.Should().BeOfType<SingleSelectCustomFieldSetupAggregate>();
            var setup = (SingleSelectCustomFieldSetupAggregate)customFieldSetup;
            var index = 0;
            setup.Options
                .Zip(command.SingleSelectCustomFieldSetup!.Options)
                .Should().AllSatisfy(x => AssertSingleSelectOption(x.First, x.Second, index++));
            if (command.SingleSelectCustomFieldSetup.DefaultOptionId is null)
                setup.DefaultOption.Should().BeNull();
            else
            {
                var optionIndex = command.SingleSelectCustomFieldSetup.Options
                    .FindIndex(o => o.Id == command.SingleSelectCustomFieldSetup.DefaultOptionId);
                setup.DefaultOption.Should().Be(setup.Options[optionIndex]);
            }
        }

        private static void AssertTextCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupCommand command)
        {
            customFieldSetup.Should().BeOfType<TextCustomFieldSetupAggregate>();
            var setup = (TextCustomFieldSetupAggregate)customFieldSetup;
            if (command.TextCustomFieldSetup!.DefaultText is null)
                setup.DefaultText.Should().BeNull();
            else
                setup.DefaultText!.Value.Should().Be(command.TextCustomFieldSetup.DefaultText);
        }

        private static void AssertSingleSelectOption(
            SingleSelectOption singleSelectOption,
            CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand
                singleSelectOptionCommand,
            int index)
        {
            singleSelectOption.Value.Should().Be(singleSelectOptionCommand.Value);
            singleSelectOption.Color.Should().Be(singleSelectOptionCommand.Color);
            singleSelectOption.OrderPosition.Should().Be(index);
        }
    }
}