using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class CustomFieldSetup
    {
        internal static bool AssertFromCreateCommand(
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command));
            }

            return true;
        }

        private static void AssertNumberCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupCommand command)
        {
            customFieldSetup.Should().BeOfType<NumberCustomFieldSetupAggregate>();
            var setup = (NumberCustomFieldSetupAggregate)customFieldSetup;
            setup.Settings.CurrencyCode.Should().Be(command.NumberSettings!.CurrencyCode);
            setup.Settings.Decimals.Should().Be((short)command.NumberSettings.Decimals);
            setup.Settings.Rounding.Should().Be(command.NumberSettings.Rounding);
        }

        private static void AssertSingleSelectCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupCommand command)
        {
            customFieldSetup.Should().BeOfType<SingleSelectCustomFieldSetupAggregate>();
            var setup = (SingleSelectCustomFieldSetupAggregate)customFieldSetup;
            var index = 0;
            setup.Options
                .Zip(command.SingleSelectOptions!)
                .Should().AllSatisfy(x => AssertSingleSelectOption(x.First, x.Second, index++));
        }

        private static void AssertSingleSelectOption(
            SingleSelectOption singleSelectOption,
            CreateCustomFieldSetupCommand.SingleSelectOptionCommand singleSelectOptionCommand,
            int index)
        {
            singleSelectOption.Value.Should().Be(singleSelectOptionCommand.Value);
            singleSelectOption.Color.Should().Be(singleSelectOptionCommand.Color);
            singleSelectOption.CustomOrderPosition.Should().Be(index);
        }
    }
}