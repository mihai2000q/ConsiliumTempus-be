using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Queries.GetOverview;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectQueryFactory
{
    public static GetProjectQuery CreateGetProjectQuery(
        Guid? id = null)
    {
        return new GetProjectQuery(id ?? Guid.NewGuid());
    }

    public static GetOverviewProjectQuery CreateGetOverviewProjectQuery(
        Guid? id = null)
    {
        return new GetOverviewProjectQuery(id ?? Guid.NewGuid());
    }

    public static GetCollectionProjectQuery CreateGetCollectionProjectQuery(
        int? pageSize = null,
        int? currentPage = null,
        string? order = null,
        Guid? workspaceId = null,
        string? name = null,
        bool? isFavorite = null,
        bool? isPrivate = null)
    {
        return new GetCollectionProjectQuery(
            pageSize,
            currentPage,
            order,
            workspaceId,
            name,
            isFavorite,
            isPrivate);
    }

    public static GetCollectionProjectForUserQuery CreateGetCollectionProjectForUserQuery()
    {
        return new GetCollectionProjectForUserQuery();
    }
}