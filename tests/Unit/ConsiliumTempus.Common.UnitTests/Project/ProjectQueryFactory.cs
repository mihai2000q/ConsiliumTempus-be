using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectQueryFactory
{
    public static GetCollectionProjectForUserQuery CreateGetCollectionProjectForUserQuery()
    {
        return new GetCollectionProjectForUserQuery();
    }
    
    public static GetCollectionProjectForWorkspaceQuery CreateGetCollectionProjectForWorkspaceQuery(
        Guid? workspaceId = null)
    {
        return new GetCollectionProjectForWorkspaceQuery(workspaceId ?? Guid.NewGuid());
    }
}