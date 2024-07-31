using ConsiliumTempus.Application.Project.Commands.UpdateOwner;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class UpdateOwnerProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateOwnerProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateOwnerProjectCommand();
            Add(command);

            command = new UpdateOwnerProjectCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateOwnerProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateOwnerProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidOwnerIdCommands : TheoryData<UpdateOwnerProjectCommand, string>
    {
        public GetInvalidOwnerIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateOwnerProjectCommand(ownerId: Guid.Empty);
            Add(command, nameof(command.OwnerId));
        }
    }
}