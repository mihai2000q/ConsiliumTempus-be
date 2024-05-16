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
            var command = WorkspaceCommandFactory.CreateUpdateWorkspaceCommand();
            Add(command);

            command = new UpdateWorkspaceCommand(
                Guid.NewGuid(),
                "Basketball Team",
                "This is the team's workspace");
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<UpdateWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<UpdateWorkspaceCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateWorkspaceCommand(name: "");
            Add(command, nameof(command.Name));

            command = WorkspaceCommandFactory.CreateUpdateWorkspaceCommand(
                name: new string('a', PropertiesValidation.Workspace.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}