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
            customFieldSetup.DomainEvents.Should().HaveCount(1);
            var domainEvent = customFieldSetup.DomainEvents[0];
            domainEvent.Should().BeOfType<CustomFieldSetupCreated>();
            ((CustomFieldSetupCreated)domainEvent).CustomFieldSetup.Should().Be(customFieldSetup);

            if (command.WorkspaceId is not null)
                customFieldSetup.Workspace.Should().Be(workspace);
            else
                customFieldSetup.Workspace.Should().BeNull();

            if (command.ProjectId is not null)
                customFieldSetup.Project.Should().Be(project);
            else
                customFieldSetup.Project.Should().BeNull();

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
        }

        internal static void AssertFromCustomFieldSetupCreated(
            CustomFieldSetupCreated domainEvent,
            List<ProjectTaskAggregate> tasks)
        {
            var setup = domainEvent.CustomFieldSetup;
            tasks.Should().AllSatisfy(task =>
            {
                task.CustomFields.Should().HaveCount(1);
                var customField = task.CustomFields[0];

                customField.Id.Value.Should().NotBeEmpty();
                switch (setup)
                {
                    case NumberCustomFieldSetupAggregate:
                        customField.Should().BeOfType<NumberCustomField>();
                        ((NumberCustomField)customField).Number.Should().BeNull();
                        ((NumberCustomField)customField).Setup.Should().Be(setup);
                        break;
                    case SingleSelectCustomFieldSetupAggregate:
                        customField.Should().BeOfType<SingleSelectCustomField>();
                        ((SingleSelectCustomField)customField).Option.Should().BeNull();
                        ((SingleSelectCustomField)customField).Setup.Should().Be(setup);
                        break;
                    case TextCustomFieldSetupAggregate:
                        customField.Should().BeOfType<TextCustomField>();
                        ((TextCustomField)customField).Text.Should().BeNull();
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