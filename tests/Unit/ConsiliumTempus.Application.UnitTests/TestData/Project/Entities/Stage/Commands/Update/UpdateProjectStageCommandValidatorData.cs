using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Commands.Update;

internal static class UpdateProjectStageCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateProjectStageCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectStageCommandFactory.CreateUpdateProjectStageCommand();
            Add(command);

            command = new UpdateProjectStageCommand(
                Guid.NewGuid(),
                "In Progress");
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateProjectStageCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectStageCommandFactory.CreateUpdateProjectStageCommand(
                id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<UpdateProjectStageCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectStageCommandFactory.CreateUpdateProjectStageCommand(
                name: "");
            Add(command, nameof(command.Name));

            command = ProjectStageCommandFactory.CreateUpdateProjectStageCommand(
                name: new string('*', PropertiesValidation.ProjectStage.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}