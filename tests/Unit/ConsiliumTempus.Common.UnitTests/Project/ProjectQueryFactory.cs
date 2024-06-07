using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Application.Project.Queries.GetStatuses;

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
        string[]? orderBy = null,
        string[]? search = null,
        Guid? workspaceId = null)
    {
        return new GetCollectionProjectQuery(
            pageSize,
            currentPage,
            orderBy,
            search,
            workspaceId);
    }
    
    public static GetStatusesFromProjectQuery CreateGetStatusesFromProjectQuery(
        Guid? id = null)
    {
        return new GetStatusesFromProjectQuery(
            id ?? Guid.NewGuid());
    }
}