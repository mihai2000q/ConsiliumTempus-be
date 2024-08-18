using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
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
                    AssertCreateNumberCustomFieldSetup(customFieldSetup, command);
                    break;
                case CustomFieldType.SingleSelect:
                    AssertCreateSingleSelectCustomFieldSetup(customFieldSetup, command);
                    break;
                case CustomFieldType.Text:
                    AssertCreateTextCustomFieldSetup(customFieldSetup, command);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command));
            }

            workspace?.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project?.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project?.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
        
        internal static void AssertRemoveFromProjectCommand(
            RemoveCustomFieldSetupFromProjectCommand command,
            CustomFieldSetupAggregate customFieldSetup,
            ProjectAggregate project)
        {
            project.Id.Value.Should().Be(command.ProjectId);
            customFieldSetup.Id.Value.Should().Be(command.Id);
            customFieldSetup.Projects.Should().NotContain(project);
            customFieldSetup.DomainEvents.Should().HaveCount(1);
            var domainEvent = customFieldSetup.DomainEvents[0];
            domainEvent.Should().BeOfType<RemovedCustomFieldSetupFromProject>();
            ((RemovedCustomFieldSetupFromProject)domainEvent).CustomFieldSetup.Should().Be(customFieldSetup);
            ((RemovedCustomFieldSetupFromProject)domainEvent).Project.Should().Be(project);

            customFieldSetup.Workspace!.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
        }

        internal static void AssertFromUpdateCommand(
            UpdateCustomFieldSetupCommand command,
            CustomFieldSetupAggregate customFieldSetup,
            UserAggregate user)
        {
            customFieldSetup.Id.Value.Should().Be(command.Id);
            customFieldSetup.Name.Value.Should().Be(command.Name);
            customFieldSetup.Description.Value.Should().Be(command.Description);
            customFieldSetup.Audit.ShouldBeUpdated(user);

            switch (customFieldSetup)
            {
                case NumberCustomFieldSetupAggregate numberSetup:
                    AssertUpdateNumberCustomFieldSetup(numberSetup, command);
                    break;
                case SingleSelectCustomFieldSetupAggregate singleSelectSetup:
                    AssertUpdateSingleSelectCustomFieldSetup(singleSelectSetup, command);
                    break;
                case TextCustomFieldSetupAggregate textSetup:
                    AssertUpdateTextCustomFieldSetup(textSetup, command);
                    break;
            }

            customFieldSetup.Workspace?.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            foreach (var project in customFieldSetup.Projects)
            {
                project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
                project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            }
        }

        internal static void AssertFromAddedCustomFieldSetupToProject(
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

        private static void AssertCreateNumberCustomFieldSetup(
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

        private static void AssertCreateSingleSelectCustomFieldSetup(
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

        private static void AssertCreateTextCustomFieldSetup(
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
            CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand
                singleSelectOptionCommand,
            int index)
        {
            singleSelectOption.Value.Should().Be(singleSelectOptionCommand.Value);
            singleSelectOption.Color.Should().Be(singleSelectOptionCommand.Color);
            singleSelectOption.OrderPosition.Should().Be(index);
        }
        
        private static void AssertUpdateNumberCustomFieldSetup(
            NumberCustomFieldSetupAggregate setup,
            UpdateCustomFieldSetupCommand command)
        {
            setup.Settings.CurrencyCode.Should().Be(command.NumberCustomFieldSetup!.Settings.CurrencyCode);
            setup.Settings.Decimals.Should().Be((short)command.NumberCustomFieldSetup!.Settings.Decimals);
            setup.Settings.Rounding.Should().Be(command.NumberCustomFieldSetup!.Settings.Rounding);
            if (command.NumberCustomFieldSetup.DefaultNumber is null)
                setup.DefaultNumber.Should().BeNull();
            else
                setup.DefaultNumber!.Value.Should().Be(command.NumberCustomFieldSetup.DefaultNumber);
        }

        private static void AssertUpdateSingleSelectCustomFieldSetup(
            SingleSelectCustomFieldSetupAggregate setup,
            UpdateCustomFieldSetupCommand command)
        {
            var defaultOption = setup.Options
                .SingleOrDefault(o => o.Id == command.SingleSelectCustomFieldSetup!.DefaultOptionId);
            setup.DefaultOption.Should().Be(defaultOption);
        }

        private static void AssertUpdateTextCustomFieldSetup(
            TextCustomFieldSetupAggregate setup,
            UpdateCustomFieldSetupCommand command)
        {
            if (command.TextCustomFieldSetup!.DefaultText is null)
                setup.DefaultText.Should().BeNull();
            else
                setup.DefaultText!.Value.Should().Be(command.TextCustomFieldSetup.DefaultText);
        }
    }
}