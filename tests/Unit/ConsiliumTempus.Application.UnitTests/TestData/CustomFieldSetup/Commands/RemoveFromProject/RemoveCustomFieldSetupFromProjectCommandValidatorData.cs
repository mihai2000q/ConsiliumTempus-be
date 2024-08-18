using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.RemoveFromProject;

internal static class RemoveCustomFieldSetupFromProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<RemoveCustomFieldSetupFromProjectCommand>
    {
        public GetValidCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateRemoveCustomFieldSetupFromProjectCommand();
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateRemoveCustomFieldSetupFromProjectCommand(Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<RemoveCustomFieldSetupFromProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateRemoveCustomFieldSetupFromProjectCommand(
                id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidProjectIdCommands : TheoryData<RemoveCustomFieldSetupFromProjectCommand, string>
    {
        public GetInvalidProjectIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateRemoveCustomFieldSetupFromProjectCommand(
                projectId: Guid.Empty);
            Add(command, nameof(command.ProjectId));
        }
    }
}