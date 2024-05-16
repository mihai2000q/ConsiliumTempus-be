using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Create;

internal static class CreateProjectTaskCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateProjectTaskCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand();
            Add(command);

            command = new CreateProjectTaskCommand(
                Guid.NewGuid(),
                "New Task",
                true);
            Add(command);
        }
    }

    internal class GetInvalidProjectStageIdCommands : TheoryData<CreateProjectTaskCommand, string>
    {
        public GetInvalidProjectStageIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand(projectStageId: Guid.Empty);
            Add(command, nameof(command.ProjectStageId));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<CreateProjectTaskCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand(name: "");
            Add(command, nameof(command.Name));

            command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand(
                name: new string('a', PropertiesValidation.ProjectTask.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}