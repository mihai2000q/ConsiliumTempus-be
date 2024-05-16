using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Commands.Delete;

internal static class DeleteProjectStageCommandValidatorData
{
    internal class GetValidCommands : TheoryData<DeleteProjectStageCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectStageCommandFactory.CreateDeleteProjectStageCommand();
            Add(command);

            command = new DeleteProjectStageCommand(Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<DeleteProjectStageCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectStageCommandFactory.CreateDeleteProjectStageCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}