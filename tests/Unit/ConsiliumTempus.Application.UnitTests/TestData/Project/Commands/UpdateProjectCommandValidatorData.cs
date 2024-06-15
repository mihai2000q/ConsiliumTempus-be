using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project.Enums;

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
                ProjectLifecycle.Archived.ToString().ToLower(),
                true);
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateProjectCommand, string, short>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id), 1);
        }
    }

    internal class GetInvalidNameCommands : TheoryData<UpdateProjectCommand, string, short>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateProjectCommand(name: "");
            Add(command, nameof(command.Name), 1);

            command = ProjectCommandFactory.CreateUpdateProjectCommand(
                name: new string('*', PropertiesValidation.Project.NameMaximumLength + 1));
            Add(command, nameof(command.Name), 1);
        }
    }

    internal class GetInvalidLifecycleCommands : TheoryData<UpdateProjectCommand, string, short>
    {
        public GetInvalidLifecycleCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateProjectCommandWithLifecycle("");
            Add(command, nameof(command.Lifecycle), 2);

            command = ProjectCommandFactory.CreateUpdateProjectCommandWithLifecycle("NotALifecycle");
            Add(command, nameof(command.Lifecycle), 1);
        }
    }
}