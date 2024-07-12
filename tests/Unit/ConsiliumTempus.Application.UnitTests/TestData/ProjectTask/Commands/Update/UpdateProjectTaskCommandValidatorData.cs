using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Update;

internal static class UpdateProjectTaskCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateProjectTaskCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand();
            Add(command);

            command = new UpdateProjectTaskCommand(
                Guid.NewGuid(),
                "New Project Task Name",
                true,
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateProjectTaskCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<UpdateProjectTaskCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand(name: "");
            Add(command, nameof(command.Name));

            command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand(
                name: new string('*', PropertiesValidation.ProjectTask.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}