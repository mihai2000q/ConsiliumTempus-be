using ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.UpdateWorkspace;

internal static class UpdateWorkspaceFromCustomFieldSetupCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateWorkspaceCustomFieldSetupCommand>
    {
        public GetValidCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateWorkspaceCustomFieldSetupCommand();
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateWorkspaceCustomFieldSetupCommand(Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateWorkspaceCustomFieldSetupCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateWorkspaceCustomFieldSetupCommand(
                id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
    
    internal class GetInvalidWorkspaceIdCommands : TheoryData<UpdateWorkspaceCustomFieldSetupCommand, string>
    {
        public GetInvalidWorkspaceIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateWorkspaceCustomFieldSetupCommand(
                workspaceId: Guid.Empty);
            Add(command, nameof(command.WorkspaceId));
        }
    }
}