using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateIsCompleted;

internal static class UpdateIsCompletedProjectTaskCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateIsCompletedProjectTaskCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateIsCompletedProjectTaskCommand();
            Add(command);

            command = new UpdateIsCompletedProjectTaskCommand(
                Guid.NewGuid(),
                true);
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateIsCompletedProjectTaskCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateIsCompletedProjectTaskCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}