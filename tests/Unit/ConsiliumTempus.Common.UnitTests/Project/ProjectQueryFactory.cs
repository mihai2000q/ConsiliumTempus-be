using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;

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

    public static GetCollectionProjectForWorkspaceQuery CreateGetCollectionProjectForWorkspaceQuery(
        Guid? workspaceId = null,
        string? name = null,
        bool? isFavorite = null,
        bool? isPrivate = null)
    {
        return new GetCollectionProjectForWorkspaceQuery(
            workspaceId ?? Guid.NewGuid(),
            name,
            isFavorite,
            isPrivate);
    }
}