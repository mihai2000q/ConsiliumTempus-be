using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectQueryFactory
{
    public static GetCollectionProjectForUserQuery CreateGetCollectionProjectForUserQuery()
    {
        return new GetCollectionProjectForUserQuery();
    }
}