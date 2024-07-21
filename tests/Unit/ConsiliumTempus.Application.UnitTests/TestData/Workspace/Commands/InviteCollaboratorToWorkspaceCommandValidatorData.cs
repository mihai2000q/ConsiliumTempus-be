using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class InviteCollaboratorToWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<InviteCollaboratorToWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand();
            Add(command);

            command = new InviteCollaboratorToWorkspaceCommand(
                Guid.NewGuid(),
                "michaelJ@Gmail.com");
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<InviteCollaboratorToWorkspaceCommand, string, short>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id), 1);
        }
    }

    internal class GetInvalidEmailCommands : TheoryData<InviteCollaboratorToWorkspaceCommand, string, short>
    {
        public GetInvalidEmailCommands()
        {
            var command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand(email: "");
            Add(command, nameof(command.Email), 2);
            
            command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand(email: "random string");
            Add(command, nameof(command.Email), 1);
            
            command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand(email: "SomeExample");
            Add(command, nameof(command.Email), 1);
            
            command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand(email: "Some@Example");
            Add(command, nameof(command.Email), 1);
            
            command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand(email: "SomeExample.com");
            Add(command, nameof(command.Email), 1);

            command = WorkspaceCommandFactory.CreateInviteCollaboratorToWorkspaceCommand(
                email: new string('a', PropertiesValidation.User.EmailMaximumLength + 1));
            Add(command, nameof(command.Email), 2);
        }
    }
}