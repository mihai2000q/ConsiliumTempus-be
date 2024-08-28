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

            customFieldSetup.Workspace!.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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

            switch (command.Type)
            {
                case CustomFieldType.Date:
                    customFieldSetup.Should().BeOfType<DateCustomFieldSetupAggregate>();
                    AssertCreateDateCustomFieldSetup(
                        (DateCustomFieldSetupAggregate)customFieldSetup,
                        command);
                    break;
                
                case CustomFieldType.DateTime:
                    customFieldSetup.Should().BeOfType<DateTimeCustomFieldSetupAggregate>();
                    AssertCreateDateTimeCustomFieldSetup(
                        (DateTimeCustomFieldSetupAggregate)customFieldSetup,
                        command);
                    break;
                
                case CustomFieldType.Duration:
                    customFieldSetup.Should().BeOfType<DurationCustomFieldSetupAggregate>();
                    AssertCreateDurationCustomFieldSetup(
                        (DurationCustomFieldSetupAggregate)customFieldSetup,
                        command);
                    break;
                
                case CustomFieldType.MultiSelect:
                    customFieldSetup.Should().BeOfType<MultiSelectCustomFieldSetupAggregate>();
                    AssertCreateMultiSelectCustomFieldSetup(
                        (MultiSelectCustomFieldSetupAggregate)customFieldSetup,
                        command);
                    break;
                
                case CustomFieldType.Number:
                    customFieldSetup.Should().BeOfType<NumberCustomFieldSetupAggregate>();
                    AssertCreateNumberCustomFieldSetup(
                        (NumberCustomFieldSetupAggregate)customFieldSetup,
                        command);
                    break;
                
                case CustomFieldType.People:
                    customFieldSetup.Should().BeOfType<PeopleCustomFieldSetupAggregate>();
                    AssertCreatePeopleCustomFieldSetup(
                        (PeopleCustomFieldSetupAggregate)customFieldSetup,
                        command);
                    break;
                
                case CustomFieldType.SingleSelect:
                    customFieldSetup.Should().BeOfType<SingleSelectCustomFieldSetupAggregate>();
                    AssertCreateSingleSelectCustomFieldSetup(
                        (SingleSelectCustomFieldSetupAggregate)customFieldSetup, 
                        command);
                    break;
                
                case CustomFieldType.Text:
                    customFieldSetup.Should().BeOfType<TextCustomFieldSetupAggregate>();
                    AssertCreateTextCustomFieldSetup(
                        (TextCustomFieldSetupAggregate)customFieldSetup,
                        command);
                    break;
                
                case CustomFieldType.Time:
                    customFieldSetup.Should().BeOfType<TimeCustomFieldSetupAggregate>();
                    AssertCreateTimeCustomFieldSetup(
                        (TimeCustomFieldSetupAggregate)customFieldSetup,
                        command);
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

            customFieldSetup.Workspace!.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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
        
        private static void AssertCreateDateCustomFieldSetup(
            DateCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            setup.DefaultDate.Should().Be(command.DateCustomFieldSetup!.DefaultDate);
        }
        
        private static void AssertCreateDateTimeCustomFieldSetup(
            DateTimeCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            setup.DefaultDateTime.Should().Be(command.DateTimeCustomFieldSetup!.DefaultDateTime);
        }
        
        private static void AssertCreateDurationCustomFieldSetup(
            DurationCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            setup.DefaultDuration.Should().Be(command.DurationCustomFieldSetup!.DefaultDuration);
        }
        
        private static void AssertCreateMultiSelectCustomFieldSetup(
            MultiSelectCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            var index = 0;
            setup.Options
                .Zip(command.MultiSelectCustomFieldSetup!.Options)
                .Should().AllSatisfy(x => AssertCreateMultiSelectOption(x.First, x.Second, index++));
        }

        private static void AssertCreateNumberCustomFieldSetup(
            NumberCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            setup.Settings.CurrencyCode.Should().Be(command.NumberCustomFieldSetup!.Settings.CurrencyCode);
            setup.Settings.Decimals.Should().Be((short)command.NumberCustomFieldSetup!.Settings.Decimals);
            setup.Settings.Rounding.Should().Be(command.NumberCustomFieldSetup!.Settings.Rounding);
            if (command.NumberCustomFieldSetup.DefaultNumber is null)
                setup.DefaultNumber.Should().BeNull();
            else
                setup.DefaultNumber!.Value.Should().Be(command.NumberCustomFieldSetup.DefaultNumber);
        }
        
        private static void AssertCreatePeopleCustomFieldSetup(
            PeopleCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            setup.Should().NotBeNull(); // obv true
            command.Should().NotBeNull();
        }

        private static void AssertCreateSingleSelectCustomFieldSetup(
            SingleSelectCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            var index = 0;
            setup.Options
                .Zip(command.SingleSelectCustomFieldSetup!.Options)
                .Should().AllSatisfy(x => AssertCreateSingleSelectOption(x.First, x.Second, index++));
            if (command.SingleSelectCustomFieldSetup.DefaultOptionId is null)
                setup.DefaultOption.Should().BeNull();
            else
            {
                var optionIndex = command.SingleSelectCustomFieldSetup.Options
                    .FindIndex(o => o.Id == command.SingleSelectCustomFieldSetup.DefaultOptionId);
                setup.DefaultOption.Should().Be(setup.Options[optionIndex]);
                setup.DefaultOptionId.Should().Be(setup.Options[optionIndex].Id);
            }
        }

        private static void AssertCreateTextCustomFieldSetup(
            TextCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            if (command.TextCustomFieldSetup!.DefaultText is null)
                setup.DefaultText.Should().BeNull();
            else
                setup.DefaultText!.Value.Should().Be(command.TextCustomFieldSetup.DefaultText);
        }
        
        private static void AssertCreateTimeCustomFieldSetup(
            TimeCustomFieldSetupAggregate setup,
            CreateCustomFieldSetupCommand command)
        {
            setup.DefaultTime.Should().Be(command.TimeCustomFieldSetup!.DefaultTime);
        }
        
        private static void AssertCreateMultiSelectOption(
            MultiSelectOption multiSelectOption,
            CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand command,
            int customOrderPosition)
        {
            multiSelectOption.Value.Should().Be(command.Value);
            multiSelectOption.Color.Should().Be(command.Color);
            multiSelectOption.CustomOrderPosition.Value.Should().Be(customOrderPosition);
        }

        private static void AssertCreateSingleSelectOption(
            SingleSelectOption singleSelectOption,
            CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand command,
            int customOrderPosition)
        {
            singleSelectOption.Id.ToString().Should().NotBe(command.Id);
            singleSelectOption.Value.Should().Be(command.Value);
            singleSelectOption.Color.Should().Be(command.Color);
            singleSelectOption.CustomOrderPosition.Value.Should().Be(customOrderPosition);
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

            if (command.SingleSelectCustomFieldSetup!.Operation is null) return;

            switch (command.SingleSelectCustomFieldSetup.Operation)
            {
                case UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add:
                    setup.Options.ShouldBeOrdered();
                    setup.Options[^1].Value.Should().Be(command.SingleSelectCustomFieldSetup.NewOption!.Value);
                    setup.Options[^1].Color.Should().Be(command.SingleSelectCustomFieldSetup.NewOption!.Color);
                    setup.Options[^1].CustomOrderPosition.Value.Should().Be(setup.Options.Count - 1);
                    break;

                case UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Update:
                    var option = setup.Options
                        .SingleOrDefault(o => o.Id == command.SingleSelectCustomFieldSetup!.OptionId);
                    option.Should().NotBeNull();
                    option!.Value.Should().Be(command.SingleSelectCustomFieldSetup.NewOption!.Value);
                    option.Color.Should().Be(command.SingleSelectCustomFieldSetup.NewOption!.Color);
                    break;

                case UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Move:
                    setup.Options.ShouldBeOrdered();
                    setup.Options.Should().Contain(o => o.Id == command.SingleSelectCustomFieldSetup!.OptionId);
                    setup.Options.Should().Contain(o => o.Id == command.SingleSelectCustomFieldSetup!.OverOptionId);
                    break;

                case UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Remove:
                    setup.Options.ShouldBeOrdered();
                    setup.Options.Should().NotContain(o => o.Id == command.SingleSelectCustomFieldSetup!.OptionId);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }
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