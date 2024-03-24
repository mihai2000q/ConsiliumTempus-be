using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Application.User.Queries.GetId;

namespace ConsiliumTempus.Common.UnitTests.User;

public static class UserQueryFactory
{
    public static GetUserQuery CreateGetUserQuery(Guid? id = null)
    {
        return new GetUserQuery(id ?? Guid.NewGuid());
    }
    
    public static GetUserIdQuery CreateGetUserIdQuery()
    {
        return new GetUserIdQuery();
    }
}