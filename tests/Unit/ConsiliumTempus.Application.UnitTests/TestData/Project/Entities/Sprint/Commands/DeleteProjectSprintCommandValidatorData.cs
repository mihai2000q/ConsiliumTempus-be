using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Sprint.Commands;

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