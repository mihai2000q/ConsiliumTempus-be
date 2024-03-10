using ConsiliumTempus.Application.User.Queries.Get;

namespace ConsiliumTempus.Common.UnitTests.User;

public static class UserQueryFactory
{
    public static GetUserQuery CreateGetUserQuery(Guid? id = null)
    {
        return new GetUserQuery(id ?? Guid.NewGuid());
    }
}