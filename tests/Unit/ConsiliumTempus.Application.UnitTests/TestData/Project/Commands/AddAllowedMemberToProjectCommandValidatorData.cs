using ConsiliumTempus.Application.Project.Commands.AddAllowedMember;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class AddAllowedMemberToProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<AddAllowedMemberToProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateAddAllowedMemberToProjectCommand();
            Add(command);

            command = new AddAllowedMemberToProjectCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<AddAllowedMemberToProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateAddAllowedMemberToProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidCollaboratorIdIdCommands : TheoryData<AddAllowedMemberToProjectCommand, string>
    {
        public GetInvalidCollaboratorIdIdCommands()
        {
            var command = ProjectCommandFactory.CreateAddAllowedMemberToProjectCommand(collaboratorId: Guid.Empty);
            Add(command, nameof(command.CollaboratorId));
        }
    }
}