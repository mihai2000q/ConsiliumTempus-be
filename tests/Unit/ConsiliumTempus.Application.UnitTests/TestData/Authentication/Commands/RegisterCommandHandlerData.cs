using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Common.UnitTests.Authentication;

namespace ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands;

internal static class RegisterCommandHandlerData
{
    internal class GetCommands : TheoryData<RegisterCommand>
    {
        public GetCommands()
        {
            Add(AuthenticationCommandFactory.CreateRegisterCommand());
            Add(AuthenticationCommandFactory.CreateRegisterCommand(role: "Business Analyst"));
        }
    }
}