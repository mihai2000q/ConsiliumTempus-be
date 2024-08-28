using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Create;

internal static class CreateCustomFieldSetupCommandHandlerData
{
    internal class GetCommands : TheoryData<CreateCustomFieldSetupCommand>
    {
        public GetCommands()
        {
            // Workspace and Project
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                Guid.NewGuid(),
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                null,
                Guid.NewGuid(),
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command);

            // Date
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new CreateCustomFieldSetupCommand.DateCustomFieldSetupCommand(null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new CreateCustomFieldSetupCommand.DateCustomFieldSetupCommand(
                    new DateOnly(2022, 10, 10)));
            Add(command);

            // Date Time
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new CreateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand(null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new CreateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand(
                    new DateTime(2022, 10, 10, 15, 30, 30)));
            Add(command);

            // Duration
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new CreateCustomFieldSetupCommand.DurationCustomFieldSetupCommand(null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new CreateCustomFieldSetupCommand.DurationCustomFieldSetupCommand(
                    new TimeSpan(10, 3, 55)));
            Add(command);

            // Multi Select
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                [
                    new CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "High",
                        "#3322FF"),
                    new CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "Medium",
                        "#7722EF"),
                    new CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "Low",
                        "#99G244")
                ]));
            Add(command);

            // Number
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        2,
                        true),
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "EUR",
                        3,
                        false),
                    12));
            Add(command);

            // People
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.People);
            Add(command);

            // Single Select
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    [
                        new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "1",
                            "High",
                            "#3322FF"),
                        new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "2",
                            "Medium",
                            "#7722EF"),
                        new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "3",
                            "Low",
                            "#99G244")
                    ],
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    [
                        new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "1",
                            "High",
                            "#3322FF"),
                        new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "2",
                            "Medium",
                            "#7722EF"),
                        new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "3",
                            "Low",
                            "#99G244")
                    ],
                    "1"
                ));
            Add(command);

            // Text
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Text,
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Text,
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand("Default Text"));
            Add(command);

            // Time
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new CreateCustomFieldSetupCommand.TimeCustomFieldSetupCommand(null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new CreateCustomFieldSetupCommand.TimeCustomFieldSetupCommand(
                    new TimeOnly(10, 25, 55)));
            Add(command);
        }
    }
}