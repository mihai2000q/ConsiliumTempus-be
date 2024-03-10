using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class UpdateWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateWorkspaceCommand>
    {
        public GetValidCommands()
        {
            Add(WorkspaceCommandFactory.CreateUpdateWorkspaceCommand());
            Add(new UpdateWorkspaceCommand(
                Guid.NewGuid(),
                "Basketball Team",
                "This is the team's workspace"));
        }
    }
    
    internal class GetInvalidNameCommands : TheoryData<UpdateWorkspaceCommand, string, int>
    {
        public GetInvalidNameCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateWorkspaceCommand(name: "");
            Add(command, nameof(command.Name), 1);

            command = WorkspaceCommandFactory.CreateUpdateWorkspaceCommand(
                name: new string('a', PropertiesValidation.Workspace.NameMaximumLength + 1));
            Add(command, nameof(command.Name), 1);
        }
    }

    internal class GetInvalidDescriptionCommands : TheoryData<UpdateWorkspaceCommand, string, int>
    {
        public GetInvalidDescriptionCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateWorkspaceCommand(
                description: new string('a', PropertiesValidation.Workspace.DescriptionMaximumLength + 1));
            Add(command, nameof(command.Description), 1);
        }
    }    
}