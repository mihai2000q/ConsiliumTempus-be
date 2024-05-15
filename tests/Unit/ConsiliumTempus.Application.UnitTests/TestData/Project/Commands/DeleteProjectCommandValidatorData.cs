using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class DeleteProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<DeleteProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateDeleteProjectCommand();
            Add(command);
            
            command = new DeleteProjectCommand(Guid.NewGuid());
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<DeleteProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateDeleteProjectCommand(Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}