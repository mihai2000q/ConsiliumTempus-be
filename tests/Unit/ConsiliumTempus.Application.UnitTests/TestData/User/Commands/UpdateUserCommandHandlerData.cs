using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Common.UnitTests.User;

namespace ConsiliumTempus.Application.UnitTests.TestData.User.Commands;

internal static class UpdateUserCommandHandlerData
{
    internal class GetCommands : TheoryData<UpdateUserCommand>
    {
        public GetCommands()
        {
            Add(UserCommandFactory.CreateUpdateUserCommand());
            Add(UserCommandFactory.CreateUpdateUserCommand(role: "Project Manager"));
        }
    }
}