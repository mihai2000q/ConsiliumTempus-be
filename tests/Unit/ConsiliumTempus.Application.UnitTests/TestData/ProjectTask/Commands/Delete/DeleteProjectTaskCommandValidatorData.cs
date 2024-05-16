using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Delete;

internal static class DeleteProjectTaskCommandValidatorData
{
    internal class GetValidCommands : TheoryData<DeleteProjectTaskCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand();
            Add(command);

            command = new DeleteProjectTaskCommand(
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<DeleteProjectTaskCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}