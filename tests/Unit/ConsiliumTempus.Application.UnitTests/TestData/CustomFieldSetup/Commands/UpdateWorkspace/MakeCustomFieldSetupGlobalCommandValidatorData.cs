using ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.UpdateWorkspace;

internal static class MakeCustomFieldSetupGlobalCommandValidatorData
{
    internal class GetValidCommands : TheoryData<MakeCustomFieldSetupGlobalCommand>
    {
        public GetValidCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateMakeCustomFieldSetupGlobalCommand();
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateMakeCustomFieldSetupGlobalCommand(Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<MakeCustomFieldSetupGlobalCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateMakeCustomFieldSetupGlobalCommand(
                id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}