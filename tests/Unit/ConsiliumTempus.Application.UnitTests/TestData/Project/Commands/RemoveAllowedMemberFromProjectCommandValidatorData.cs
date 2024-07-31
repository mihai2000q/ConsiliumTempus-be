using ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class RemoveAllowedMemberFromProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<RemoveAllowedMemberFromProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand();
            Add(command);

            command = new RemoveAllowedMemberFromProjectCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<RemoveAllowedMemberFromProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidAllowedMemberIdIdCommands : TheoryData<RemoveAllowedMemberFromProjectCommand, string>
    {
        public GetInvalidAllowedMemberIdIdCommands()
        {
            var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand(allowedMemberId: Guid.Empty);
            Add(command, nameof(command.AllowedMemberId));
        }
    }
}