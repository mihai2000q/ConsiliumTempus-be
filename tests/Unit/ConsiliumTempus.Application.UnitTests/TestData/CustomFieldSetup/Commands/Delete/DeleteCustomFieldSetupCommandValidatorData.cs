using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Delete;

internal static class DeleteCustomFieldSetupCommandValidatorData
{
    internal class GetValidCommands : TheoryData<DeleteCustomFieldSetupCommand>
    {
        public GetValidCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateDeleteCustomFieldSetupCommand();
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateDeleteCustomFieldSetupCommand(Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<DeleteCustomFieldSetupCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateDeleteCustomFieldSetupCommand(Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}