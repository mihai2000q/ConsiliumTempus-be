using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class CreateWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateCreateWorkspaceCommand();
            Add(command);

            command = new CreateWorkspaceCommand(
                "Basketball Team");
            Add(command);
        }
    }
    
    internal class GetInvalidNameCommands : TheoryData<CreateWorkspaceCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = WorkspaceCommandFactory.CreateCreateWorkspaceCommand(name: "");
            Add(command, nameof(command.Name));

            command = WorkspaceCommandFactory.CreateCreateWorkspaceCommand(
                name: new string('a', PropertiesValidation.Workspace.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}