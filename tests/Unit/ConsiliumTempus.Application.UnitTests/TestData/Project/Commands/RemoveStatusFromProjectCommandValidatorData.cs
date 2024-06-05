using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class RemoveStatusFromProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<RemoveStatusFromProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateRemoveStatusFromProjectCommand();
            Add(command);

            command = new RemoveStatusFromProjectCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<RemoveStatusFromProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateRemoveStatusFromProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidStatusIdCommands : TheoryData<RemoveStatusFromProjectCommand, string>
    {
        public GetInvalidStatusIdCommands()
        {
            var command = ProjectCommandFactory.CreateRemoveStatusFromProjectCommand(statusId: Guid.Empty);
            Add(command, nameof(command.StatusId));
        }
    }
}