using ConsiliumTempus.Application.User.Commands.DeleteCurrent;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;

namespace ConsiliumTempus.Common.UnitTests.User;

public static class UserResultFactory
{
    public static UpdateCurrentUserResult CreateUpdateCurrentUserResult()
    {
        return new UpdateCurrentUserResult();
    }
    
    public static DeleteCurrentUserResult CreateDeleteCurrentUserResult()
    {
        return new DeleteCurrentUserResult();
    }
}