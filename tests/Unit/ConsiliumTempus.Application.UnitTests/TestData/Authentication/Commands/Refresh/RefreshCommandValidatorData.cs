using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Common.UnitTests.Authentication;

namespace ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands.Refresh;

internal static class RefreshCommandValidatorData
{
    internal class GetValidCommands : TheoryData<RefreshCommand>
    {
        public GetValidCommands()
        {
            Add(AuthenticationCommandFactory.CreateRefreshCommand());
        }
    }
    
    internal class GetInvalidTokenCommands : TheoryData<RefreshCommand, string>
    {
        public GetInvalidTokenCommands()
        {
            var command = AuthenticationCommandFactory.CreateRefreshCommand(token: "");
            Add(command, nameof(command.Token));
        }
    }
    
    internal class GetInvalidRefreshTokenCommands : TheoryData<RefreshCommand, string>
    {
        public GetInvalidRefreshTokenCommands()
        {
            var command = AuthenticationCommandFactory.CreateRefreshCommand(refreshToken: "");
            Add(command, nameof(command.RefreshToken));
        }
    }
}