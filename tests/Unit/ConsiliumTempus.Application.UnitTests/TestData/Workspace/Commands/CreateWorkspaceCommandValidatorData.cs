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
            Add(WorkspaceCommandFactory.CreateCreateWorkspaceCommand());
            Add(new CreateWorkspaceCommand(
                "Basketball Team"));
        }
    }
    
    internal class GetInvalidNameCommands : TheoryData<CreateWorkspaceCommand, string, int>
    {
        public GetInvalidNameCommands()
        {
            var command = WorkspaceCommandFactory.CreateCreateWorkspaceCommand(name: "");
            Add(command, nameof(command.Name), 1);

            command = WorkspaceCommandFactory.CreateCreateWorkspaceCommand(
                name: new string('a', PropertiesValidation.Workspace.NameMaximumLength + 1));
            Add(command, nameof(command.Name), 1);
        }
    }
}