using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class UpdateProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateProjectCommand();
            Add(command);
            
            command = new UpdateProjectCommand(
                Guid.NewGuid(),
                "New Name",
                true);
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<UpdateProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
    
    internal class GetInvalidNameCommands : TheoryData<UpdateProjectCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateProjectCommand(name: "");
            Add(command, nameof(command.Name));
            
            command = ProjectCommandFactory.CreateUpdateProjectCommand(
                name: new string('*', PropertiesValidation.Project.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}