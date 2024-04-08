using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Common.UnitTests.User;

namespace ConsiliumTempus.Application.UnitTests.TestData.User.Commands.UpdateCurrent;

internal static class UpdateCurrentUserCommandHandlerData
{
    internal class GetCommands : TheoryData<UpdateCurrentUserCommand>
    {
        public GetCommands()
        {
            Add(UserCommandFactory.CreateUpdateUserCommand());
            Add(UserCommandFactory.CreateUpdateUserCommand(role: "Project Manager"));
        }
    }
}