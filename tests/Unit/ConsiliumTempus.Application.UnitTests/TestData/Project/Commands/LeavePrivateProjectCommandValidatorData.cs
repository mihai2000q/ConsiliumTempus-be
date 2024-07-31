using ConsiliumTempus.Application.Project.Commands.LeavePrivate;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class LeavePrivateProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<LeavePrivateProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateLeavePrivateProjectCommand();
            Add(command);

            command = new LeavePrivateProjectCommand(Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<LeavePrivateProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateLeavePrivateProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}