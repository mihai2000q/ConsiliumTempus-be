using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.AddToProject;

internal static class AddCustomFieldSetupToProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<AddCustomFieldSetupToProjectCommand>
    {
        public GetValidCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand();
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand(Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<AddCustomFieldSetupToProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand(
                id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidProjectIdCommands : TheoryData<AddCustomFieldSetupToProjectCommand, string>
    {
        public GetInvalidProjectIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand(
                projectId: Guid.Empty);
            Add(command, nameof(command.ProjectId));
        }
    }
}