using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectQueryFactory
{
    public static GetProjectQuery CreateGetProjectQuery(
        Guid? projectId = null)
    {
        return new GetProjectQuery(projectId ?? Guid.NewGuid());
    }

    public static GetCollectionProjectForUserQuery CreateGetCollectionProjectForUserQuery()
    {
        return new GetCollectionProjectForUserQuery();
    }

    public static GetCollectionProjectQuery CreateGetCollectionProjectQuery(
        Guid? workspaceId = null,
        string? name = null,
        bool? isFavorite = null,
        bool? isPrivate = null)
    {
        return new GetCollectionProjectQuery(
            workspaceId,
            name,
            isFavorite,
            isPrivate);
    }
}