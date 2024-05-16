using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.Delete;

internal static class DeleteProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<DeleteProjectSprintCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectSprintCommandFactory.CreateDeleteProjectSprintCommand();
            Add(command);
            
            command = new DeleteProjectSprintCommand(Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<DeleteProjectSprintCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateDeleteProjectSprintCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}